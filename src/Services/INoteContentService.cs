using System.Threading.Tasks;

namespace NoteBin.Services
{
    public interface INoteContentService : IContentService<string>
    {
        Task<string?> GetContentPreview(string id, int length);
    }
}
