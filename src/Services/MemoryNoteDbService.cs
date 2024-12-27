using NoteBin.Models;
using NoteBin.Models.Dto;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace NoteBin.Services
{
    public class MemoryNoteDbService : INoteDbService
    {
        private readonly INoteIdGenService idGenService;
        private ConcurrentDictionary<string, Note> notes = new ConcurrentDictionary<string, Note>();

        public MemoryNoteDbService(INoteIdGenService idGenService)
        {
            this.idGenService = idGenService;
        }

        public Task<Note?> GetNote(string id)
        {
            if(notes.TryGetValue(id, out Note? note))
            {
                return Task.FromResult<Note?>(note);
            }
            return Task.FromResult<Note?>(null);
        }

        public Task<Note?> SaveNote(NoteCreateDto createDto)
        {
            if(createDto.Syntax == null || createDto.Content == null)
            {
                return Task.FromResult<Note?>(null);
            }

            Note note;
            do
            {
                string id = idGenService.GenerateId();
                note = new Note(id, DateTime.Now, createDto.Syntax, createDto.Content);
            }
            while(!notes.TryAdd(note.Id, note));

            return Task.FromResult<Note?>(note);
        }
    }
}
