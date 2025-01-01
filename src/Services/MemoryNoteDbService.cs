using NoteBin.Models;
using NoteBin.Models.API;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace NoteBin.Services
{
    public class MemoryNoteDbService : INoteDbService
    {
        private readonly INoteIdGenService idGenService;
        private readonly INoteContentService contentService;
        private ConcurrentDictionary<string, Note> notes = new ConcurrentDictionary<string, Note>();

        public MemoryNoteDbService(INoteIdGenService idGenService, INoteContentService contentService)
        {
            this.idGenService = idGenService;
            this.contentService = contentService;
        }

        public async Task<Note?> GetNote(string id)
        {
            if(notes.TryGetValue(id, out Note? note))
            {
                string? content = await contentService.GetContent(id);
                if(content != null)
                {
                    note.Content = content;
                    return note;
                }
            }
            return null;
        }

        public async Task<Note?> SaveNote(NoteCreateRequest request, User? owner)
        {
            Note note;
            do
            {
                string id = idGenService.GenerateId();
                note = new Note(id, request.Name, owner?.Name, DateTime.UtcNow, request.Syntax);
            }
            while(!notes.TryAdd(note.Id, note));

            bool savedContent = await contentService.SaveContent(note.Id, request.Content);
            if(!savedContent)
            {
                notes.TryRemove(note.Id, out _);
            }

            return note;
        }
    }
}
