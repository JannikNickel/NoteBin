using NoteBin.Models;
using NoteBin.Models.API;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoteBin.Services
{
    public interface INoteDbService
    {
        Task<Note?> GetNote(string id);
        Task<Note?> SaveNote(NoteCreateRequest note, User? owner);
        Task<List<Note>> GetLatestNotes(long offset, long amount, string? user = null);
    }
}
