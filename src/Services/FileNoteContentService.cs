using System.IO;
using System.Threading.Tasks;

namespace NoteBin.Services
{
    public class FileNoteContentService : INoteContentService
    {
        private const string fileExtension = ".dat";

        private readonly string directory;

        public FileNoteContentService(string directory)
        {
            this.directory = directory;
            Directory.CreateDirectory(directory);
        }

        public async Task<string?> GetContent(string id)
        {
            string path = GetPath(id);
            if(File.Exists(path))
            {
                return await File.ReadAllTextAsync(path);
            }
            return null;
        }

        public async Task<bool> SaveContent(string id, string content)
        {
            string path = GetPath(id);
            if(File.Exists(path))
            {
                return false;
            }

            await File.WriteAllTextAsync(path, content);
            return true;
        }

        public async Task<string?> GetContentPreview(string id, int length)
        {
            string path = GetPath(id);
            if(File.Exists(path))
            {
                FileStreamOptions options = new FileStreamOptions()
                {
                    Access = FileAccess.Read,
                    Share = FileShare.Read,
                    Mode = FileMode.Open,
                    Options = FileOptions.Asynchronous
                };
                using StreamReader reader = new StreamReader(path, options);
                char[] buffer = new char[length];
                int read = await reader.ReadAsync(buffer, 0, length);
                return new string(buffer, 0, read);
            }
            return null;
        }

        private string GetPath(string id) => Path.Combine(directory, $"{id}{fileExtension}");
    }
}
