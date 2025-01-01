using System.Threading.Tasks;
using NoteBin.Models;

namespace NoteBin.Services
{
    public interface IAuthService
    {
        public Task<string> GenerateToken(User user);
        public Task<bool> ValidateToken(string token);
    }
}