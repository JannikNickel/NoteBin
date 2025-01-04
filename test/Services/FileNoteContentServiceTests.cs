using NoteBin.Services;
using System.IO;

namespace NoteBin.Test.Services
{
    [TestClass]
    public class FileNoteContentServiceTests : INoteContentServiceTests<FileNoteContentService>
    {
        private string tmpFolder = null!;

        [TestInitialize]
        public void Setup()
        {
            tmpFolder = TestHelper.TempContentDirectory();
            service = new FileNoteContentService(tmpFolder);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if(Directory.Exists(tmpFolder))
            {
                Directory.Delete(tmpFolder, true);
            }
        }
    }
}
