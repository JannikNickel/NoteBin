using NoteBin.Configuration;
using NoteBin.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NoteBin.Services
{
    public class StatelessAuthService : IAuthService
    {
        private readonly StatelessTokenGen tokenGen;
        private readonly TimeSpan expirationTime;

        public StatelessAuthService(AuthSettings settings)
        {
            byte[]? key = File.Exists(settings.KeyFile) ? File.ReadAllBytes(settings.KeyFile) : null;
            if(key?.Length != settings.KeyLength)
            {
                key = StatelessTokenGen.GenerateSecretKey(settings.KeyLength);
                File.WriteAllBytes(settings.KeyFile, key);
            }
            tokenGen = new StatelessTokenGen(key, settings.TokenLength);
            expirationTime = TimeSpan.FromSeconds(settings.ExpirationDuration);
        }

        public Task<string> GenerateToken(User user)
        {
            return Task.FromResult(tokenGen.Generate(user.Name, expirationTime));
        }

        public Task<bool> ValidateToken(string token)
        {
            return Task.FromResult(tokenGen.Validate(token));
        }
    }
}