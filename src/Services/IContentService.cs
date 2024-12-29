using System.Threading.Tasks;

namespace NoteBin.Services
{
    public interface IContentService<T>
    {
        Task<T?> GetContent(string id);
        Task<bool> SaveContent(string id, T content);
    }
}
