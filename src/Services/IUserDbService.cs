using NoteBin.Models;
using NoteBin.Models.API;
using System.Threading.Tasks;

namespace NoteBin.Services
{
    public interface IUserDbService
    {
        Task<User?> GetUser(string name);
        Task<User?> CreateUser(UserRequest request);
        bool VerifyUser(User user, string password);
    }
}
