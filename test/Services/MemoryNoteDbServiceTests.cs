using NoteBin.Services;

namespace NoteBin.Test.Services
{
    [TestClass]
    public class MemoryNoteDbServiceTests : INoteDbServiceTests<MemoryNoteDbService>
    {
        [TestInitialize]
        public void Setup()
        {
            idGenService = TestHelper.Services.IdGen;
            contentService = TestHelper.Services.NoteContent;
            service = new MemoryNoteDbService(idGenService, contentService);
        }
    }
}
