using NoteBin.Models;
using NoteBin.Models.API;
using NoteBin.Models.Sqlite;
using System;
using System.Data.SQLite;
using System.IO;
using System.Threading.Tasks;

namespace NoteBin.Services
{
    public class SqLiteNoteDbService : INoteDbService
    {
        private const int insertAttemptLimit = 10;

        private readonly INoteIdGenService idGenService;
        private readonly INoteContentService contentService;
        private readonly string connectionString;

        public SqLiteNoteDbService(INoteIdGenService idGenService, INoteContentService contentService, string? connectionString)
        {
            ArgumentException.ThrowIfNullOrEmpty(connectionString);

            this.idGenService = idGenService;
            this.contentService = contentService;
            this.connectionString = connectionString;

            Initialize();
        }

        private void Initialize()
        {
            SqLiteHelper.EnsureDataDirectory(connectionString);

            using SQLiteConnection connection = SqLiteHelper.Open(connectionString);
            using CreateNoteTableCmd createCmd = new CreateNoteTableCmd(connection);
            createCmd.Execute();
        }

        public async Task<Note?> GetNote(string id)
        {
            string? content = await contentService.GetContent(id);
            if(content == null)
            {
                return null;
            }

            using SQLiteConnection connection = await SqLiteHelper.OpenAsync(connectionString);
            using SelectNotesCmd selectCmd = new SelectNotesCmd(connection, id);
            Note? note = await selectCmd.ReadFirstRowAsync();
            if(note != null)
            {
                note.Content = content;
                return note;
            }
            return null;
        }

        public async Task<Note?> SaveNote(NoteCreateRequest createDto)
        {
            if(createDto.Name == null || createDto.Syntax == null || createDto.Content == null)
            {
                return null;
            }

            Note? note = null;
            bool inserted = false;
            int attempts = 0;
            while(!inserted && attempts++ < insertAttemptLimit)
            {
                string id = idGenService.GenerateId();
                note = new Note(id, createDto.Name, DateTime.UtcNow, createDto.Syntax);

                using SQLiteConnection connection = await SqLiteHelper.OpenAsync(connectionString);
                using InsertNoteCmd insertCmd = new InsertNoteCmd(connection, note);
                try
                {
                    await insertCmd.ExecuteAsync();
                    inserted = true;
                }
                catch(SQLiteException ex) when(ex.ErrorCode == (int)SQLiteErrorCode.Constraint) { }
            }

            if(inserted && note != null)
            {
                bool savedContent = await contentService.SaveContent(note.Id, createDto.Content);
                if(!savedContent)
                {
                    using SQLiteConnection connection = await SqLiteHelper.OpenAsync(connectionString);
                    using DeleteNotesCmd command = new DeleteNotesCmd(connection, note.Id);
                    await command.ExecuteAsync();
                }
            }

            return inserted ? note : null;
        }
    }
}
