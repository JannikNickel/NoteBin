using NoteBin.Models;
using NoteBin.Models.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NoteBin.Services
{
    public class MemoryNoteDbService : INoteDbService
    {
        private readonly INoteIdGenService idGenService;
        private readonly INoteContentService contentService;
        private readonly Dictionary<string, Note> notes = new Dictionary<string, Note>();
        private readonly List<Note> sortedNotes = new List<Note>();
        private readonly Lock _lock = new Lock();

        public MemoryNoteDbService(INoteIdGenService idGenService, INoteContentService contentService)
        {
            this.idGenService = idGenService;
            this.contentService = contentService;
        }

        public async Task<Note?> GetNote(string id)
        {
            string? content = await contentService.GetContent(id);
            if(content != null)
            {
                lock(_lock)
                {
                    if(notes.TryGetValue(id, out Note? note))
                    {
                        note.Content = content;
                        return note;
                    }
                }
            }
            return null;
        }

        public async Task<Note?> SaveNote(NoteCreateRequest request, User? owner)
        {
            Note note;
            lock(_lock)
            {
                do
                {
                    string id = idGenService.GenerateId();
                    note = new Note(id, request.Name, owner?.Name, request.Fork, DateTime.UtcNow, request.Syntax);
                }
                while(!notes.TryAdd(note.Id, note));
                sortedNotes.Insert(0, note);
            }

            bool savedContent = await contentService.SaveContent(note.Id, request.Content);
            if(!savedContent)
            {
                lock(_lock)
                {
                    notes.Remove(note.Id, out _);
                    sortedNotes.Remove(note);
                }
            }

            return note;
        }

        public async Task<(List<Note> notes, long total)> GetNotes(long offset, long amount, string? user = null, string? filter = null)
        {
            List<Note> result;
            long total;
            lock(_lock)
            {
                bool UserFilter(Note n) => user == null || n.Owner == user;
                bool OwnerFilter(Note n) => filter == null || (n.Name?.Contains(filter, StringComparison.OrdinalIgnoreCase) ?? false);

                result = sortedNotes
                    .Where(UserFilter)
                    .Where(OwnerFilter)
                    .Skip((int)offset)
                    .Take((int)amount)
                    .ToList();
                total = sortedNotes.Count(n => UserFilter(n) && OwnerFilter(n));
            }
            foreach(Note note in result)
            {
                note.Content = await contentService.GetContentPreview(note.Id, Constants.NotePreviewLength);
            }
            return (result, total);
        }
    }
}
