using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoteBin.Services
{
    public class MemoryNoteContentService : INoteContentService
    {
        private readonly Dictionary<string, string> storage = new Dictionary<string, string>();

        public Task<string?> GetContent(string id)
        {
            if(storage.TryGetValue(id, out string? content))
            {
                return Task.FromResult<string?>(content);
            }
            return Task.FromResult<string?>(null);
        }

        public Task<bool> SaveContent(string id, string content)
        {
            bool res = storage.TryAdd(id, content);
            return Task.FromResult(res);
        }
    }
}
