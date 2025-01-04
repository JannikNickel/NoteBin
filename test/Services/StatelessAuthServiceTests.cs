using NoteBin.Services;

namespace NoteBin.Test.Services
{
    [TestClass]
    public class StatelessAuthDbServiceTests : IAuthServiceTests<StatelessAuthService>
    {
        [TestInitialize]
        public void Setup()
        {
            userService = TestHelper.Services.UserDb;
            service = new StatelessAuthService(TestHelper.AuthSettings, userService);
        }
    }
}
