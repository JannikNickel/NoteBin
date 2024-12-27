using NoteBin.Models;
using NoteBin.Models.Dto;
using System.Threading.Tasks;

namespace NoteBin.Services
{
    public interface INoteDbService
    {
        Task<Note?> GetNote(string id);
        Task<Note?> SaveNote(NoteCreateDto note);
    }
}
