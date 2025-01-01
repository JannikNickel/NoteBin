using NoteBin.Models;
using NoteBin.Models.API;
using System.Threading.Tasks;

namespace NoteBin.Services
{
    public interface INoteDbService
    {
        Task<Note?> GetNote(string id);
        Task<Note?> SaveNote(NoteCreateRequest note, User? owner);
    }
}
