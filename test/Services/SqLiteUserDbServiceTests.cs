using NoteBin.Services;
using System.IO;

namespace NoteBin.Test.Services
{
    [TestClass]
    public class SqLiteUserDbServiceTests : IUserDbServiceTests<SqLiteUserDbService>
    {
        private string tmpFile = null!;

        [TestInitialize]
        public void Setup()
        {
            service = new SqLiteUserDbService(TestHelper.TempConnectionSource(out tmpFile));
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
