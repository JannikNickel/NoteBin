using NoteBin.Services;
using System.Threading.Tasks;

namespace NoteBin.Test.Services
{
    [TestClass]
    public abstract class INoteContentServiceTests<T> where T : INoteContentService
    {
        protected T service = default!;

        private static string TestId => "testId";
        private static string TestContent => "test content";

        [TestMethod]
        public async Task GetContent_Success()
        {
            await service.SaveContent(TestId, TestContent);
            string? result = await service.GetContent(TestId);
            Assert.IsNotNull(result);
            Assert.AreEqual(TestContent, result);
        }

        [TestMethod]
        public async Task GetContent_NotExists()
        {
            string? result = await service.GetContent("nonExistentId");
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task SaveContent_Success()
        {
            bool result = await service.SaveContent(TestId, TestContent);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task GetContentPreview_Success()
        {
            const int length = 5;
            await service.SaveContent(TestId, TestContent);
            string? preview = await service.GetContentPreview(TestId, length);
            Assert.AreEqual(TestContent[..length], preview);
        }

        [TestMethod]
        public async Task GetContentPreview_NotExists()
        {
            string? preview = await service.GetContentPreview("nonExistentId", 5);
            Assert.IsNull(preview);
        }
    }
}
