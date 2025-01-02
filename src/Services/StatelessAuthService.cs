using NoteBin.Configuration;
using NoteBin.Models;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;

namespace NoteBin.Services
{
    public class StatelessAuthService : IAuthService
    {
        private readonly StatelessTokenGen tokenGen;
        private readonly TimeSpan expirationTime;
        private readonly IUserDbService userService;

        public StatelessAuthService(AuthSettings settings, IUserDbService userService)
        {
            byte[]? key = File.Exists(settings.KeyFile) ? File.ReadAllBytes(settings.KeyFile) : null;
            if(key?.Length != settings.KeyLength)
            {
                key = StatelessTokenGen.GenerateSecretKey(settings.KeyLength);
                File.WriteAllBytes(settings.KeyFile, key);
            }
            tokenGen = new StatelessTokenGen(key, settings.TokenLength);
            expirationTime = TimeSpan.FromSeconds(settings.ExpirationDuration);
            this.userService = userService;
        }

        public Task<string> GenerateToken(User user)
        {
            return Task.FromResult(tokenGen.Generate(user.Name, expirationTime));
        }

        public async Task<User?> ValidateToken(string token)
        {
            if(tokenGen.Validate(token, out string? username))
            {
                return await userService.GetUser(username);
            }
            return null;
        }

        public async Task<bool> Logout(string token)
        {
            //Just validate for stateless auth... client will forget the token
            return await ValidateToken(token) != null;
        }
    }
}