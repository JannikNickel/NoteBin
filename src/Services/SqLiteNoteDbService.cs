using NoteBin.Models;
using NoteBin.Models.API;
using NoteBin.Models.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace NoteBin.Services
{
    public class SqLiteNoteDbService : INoteDbService
    {
        private const int InsertAttemptLimit = 10;

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
            using SelectNoteCmd selectCmd = new SelectNoteCmd(connection, id);
            Note? note = await selectCmd.ReadFirstRowAsync();
            if(note != null)
            {
                note.Content = content;
                return note;
            }
            return null;
        }

        public async Task<Note?> SaveNote(NoteCreateRequest request, User? owner)
        {
            Note? note = null;
            bool inserted = false;
            int attempts = 0;
            while(!inserted && attempts++ < InsertAttemptLimit)
            {
                string id = idGenService.GenerateId();
                note = new Note(id, request.Name, owner?.Name, request.Fork, DateTime.UtcNow, request.Syntax);

                using SQLiteConnection connection = await SqLiteHelper.OpenAsync(connectionString);
                using InsertNoteCmd insertCmd = new InsertNoteCmd(connection, note);
                inserted = await insertCmd.ExecuteAsync() == SQLiteErrorCode.Ok;
            }

            if(inserted && note != null)
            {
                bool savedContent = await contentService.SaveContent(note.Id, request.Content);
                if(!savedContent)
                {
                    using SQLiteConnection connection = await SqLiteHelper.OpenAsync(connectionString);
                    using DeleteNotesCmd command = new DeleteNotesCmd(connection, note.Id);
                    await command.ExecuteAsync();
                    note = null;
                }
            }

            return inserted ? note : null;
        }

        public async Task<(List<Note> notes, long total)> GetNotes(long offset, long amount, string? user = null, string? filter = null)
        {
            List<Note> notes = new List<Note>();
            using SQLiteConnection connection = await SqLiteHelper.OpenAsync(connectionString);
            using SelectNotesCmd selectCmd = new SelectNotesCmd(connection, offset, amount, user, filter, NoteSortOrder.CreationTimeDesc);
            long totalCount = await selectCmd.CountTotal();
            await foreach(Note note in selectCmd.ReadRowsAsync())
            {
                notes.Add(note);
            }

            IEnumerable<Task> previewLoading = notes.Select(async note =>
            {
                note.Content = await contentService.GetContentPreview(note.Id, Constants.NotePreviewLength);
            });
            await Task.WhenAll(previewLoading);

            return (notes, totalCount);
        }
    }
}
