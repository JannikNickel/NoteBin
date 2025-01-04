using NoteBin.Services;
using System.IO;

namespace NoteBin.Test.Services
{
    [TestClass]
    public class SqLiteNoteDbServiceTests : INoteDbServiceTests<SqLiteNoteDbService>
    {
        private string tmpFile = null!;

        [TestInitialize]
        public void Setup()
        {
            idGenService = TestHelper.Services.IdGen;
            contentService = TestHelper.Services.NoteContent;
            service = new SqLiteNoteDbService(idGenService, contentService, TestHelper.TempConnectionSource(out tmpFile));
        }

        [TestCleanup]
        public void Cleanup()
        {
            if(File.Exists(tmpFile))
            {
                File.Delete(tmpFile);
            }
        }
    }
}
