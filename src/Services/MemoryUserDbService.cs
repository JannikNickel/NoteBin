using Microsoft.AspNetCore.Identity;
using NoteBin.Models;
using NoteBin.Models.API;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace NoteBin.Services
{
    public class MemoryUserDbService : IUserDbService
    {
        private readonly PasswordHasher<string> passwordHasher = new PasswordHasher<string>();
        private readonly ConcurrentDictionary<string, User> users = new ConcurrentDictionary<string, User>();

        public Task<User?> CreateUser(UserRequest request)
        {
            if(request.Username == null || request.Password == null)
            {
                return Task.FromResult<User?>(null);
            }

            string hashedPassword = passwordHasher.HashPassword(request.Username, request.Password);
            User user = new User(request.Username, hashedPassword, DateTime.UtcNow);
            return Task.FromResult(users.TryAdd(user.Name, user) ? user : null);
        }

        public Task<User?> GetUser(string name)
        {
            if(users.TryGetValue(name, out User? user))
            {
                return Task.FromResult<User?>(user);
            }
            return Task.FromResult<User?>(null);
        }

        public bool VerifyUser(User user, string password)
        {
            return passwordHasher.VerifyHashedPassword(user.Name, user.Password, password) != PasswordVerificationResult.Failed;
        }
    }
}