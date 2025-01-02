global using UserCreationResult = NoteBin.Result<NoteBin.Models.User, NoteBin.Models.UserCreationError>;

using NoteBin.Models;
using NoteBin.Models.API;
using System.Threading.Tasks;

namespace NoteBin.Services
{
    public interface IUserDbService
    {
        Task<User?> GetUser(string name);
        Task<UserCreationResult> CreateUser(AuthRequest request);
        bool VerifyUser(User user, string password);
    }
}
