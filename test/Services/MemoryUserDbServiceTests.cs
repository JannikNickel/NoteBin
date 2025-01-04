using NoteBin.Services;

namespace NoteBin.Test.Services
{
    [TestClass]
    public class MemoryUserDbServiceTests : IUserDbServiceTests<MemoryUserDbService>
    {
        [TestInitialize]
        public void Setup()
        {
            service = new MemoryUserDbService();
        }
    }
}
