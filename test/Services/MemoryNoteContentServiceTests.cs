using NoteBin.Services;

namespace NoteBin.Test.Services
{
    [TestClass]
    public class MemoryNoteContentServiceTests : INoteContentServiceTests<MemoryNoteContentService>
    {
        [TestInitialize]
        public void Setup()
        {
            service = new MemoryNoteContentService();
        }
    }
}
