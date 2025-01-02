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

        public Task<UserCreationResult> CreateUser(UserRequest request)
        {
            if(!UserHelper.ValidateUsername(request.Username))
            {
                return Task.FromResult(UserCreationResult.Err(UserCreationError.InvalidUsername));
            }
            if(!UserHelper.ValidatePassword(request.Password))
            {
                return Task.FromResult(UserCreationResult.Err(UserCreationError.InvalidPassword));
            }

            string hashedPassword = passwordHasher.HashPassword(request.Username, request.Password);
            User user = new User(request.Username, hashedPassword, DateTime.UtcNow);
            return Task.FromResult(users.TryAdd(user.Name, user) ? UserCreationResult.Ok(user) : UserCreationResult.Err(UserCreationError.DuplicateUsername));
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