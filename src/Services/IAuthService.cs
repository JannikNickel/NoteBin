using System.Threading.Tasks;
using NoteBin.Models;

namespace NoteBin.Services
{
    public interface IAuthService
    {
        public Task<string> GenerateToken(User user);
        public Task<User?> ValidateToken(string token);
        Task<bool> Logout(string token);
    }
}