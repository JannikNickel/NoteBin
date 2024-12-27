using NoteBin.Models;
using NoteBin.Models.Dto;
using System;
using System.Threading.Tasks;

namespace NoteBin.Services
{
    public class SqLiteNoteDbService : INoteDbService
    {
        private readonly INoteIdGenService idGenService;

        public SqLiteNoteDbService(INoteIdGenService idGenService, string? connectionString)
        {
            this.idGenService = idGenService;

            if(string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException($"Invalid connectionString ({connectionString})!");
            }
        }

        public Task<Note?> GetNote(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Note?> SaveNote(NoteCreateDto note)
        {
            throw new NotImplementedException();
        }
    }
}
