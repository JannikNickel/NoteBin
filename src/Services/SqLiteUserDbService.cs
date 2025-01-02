using Microsoft.AspNetCore.Identity;
using NoteBin.Models;
using NoteBin.Models.API;
using NoteBin.Models.Sqlite;
using System;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace NoteBin.Services
{
    public class SqLiteUserDbService : IUserDbService
    {
        private readonly PasswordHasher<string> passwordHasher = new PasswordHasher<string>();
        private readonly string connectionString;

        public SqLiteUserDbService(string? connectionString)
        {
            ArgumentException.ThrowIfNullOrEmpty(connectionString);

            this.connectionString = connectionString;
            Initialize();
        }

        private void Initialize()
        {
            SqLiteHelper.EnsureDataDirectory(connectionString);

            using SQLiteConnection connection = SqLiteHelper.Open(connectionString);
            using CreateUserTableCmd createCmd = new CreateUserTableCmd(connection);
            createCmd.Execute();
        }

        public async Task<UserCreationResult> CreateUser(AuthRequest request)
        {
            if(!UserHelper.ValidateUsername(request.Username))
            {
                return UserCreationResult.Err(UserCreationError.InvalidUsername);
            }
            if(!UserHelper.ValidatePassword(request.Password))
            {
                return UserCreationResult.Err(UserCreationError.InvalidPassword);
            }

            string hashedPassword = passwordHasher.HashPassword(request.Username, request.Password);
            User user = new User(request.Username, hashedPassword, DateTime.UtcNow);

            using SQLiteConnection connection = await SqLiteHelper.OpenAsync(connectionString);
            using InsertUserCmd insertCmd = new InsertUserCmd(connection, user);
            return await insertCmd.ExecuteAsync() switch
            {
                SQLiteErrorCode.Ok => UserCreationResult.Ok(user),
                SQLiteErrorCode.Constraint => UserCreationResult.Err(UserCreationError.DuplicateUsername),
                _ => UserCreationResult.Err(UserCreationError.Unknown)
            };
        }

        public async Task<User?> GetUser(string name)
        {
            using SQLiteConnection connection = await SqLiteHelper.OpenAsync(connectionString);
            using SelectUsersCmd selectCmd = new SelectUsersCmd(connection, name);
            User? user = await selectCmd.ReadFirstRowAsync();
            return user;
        }

        public bool VerifyUser(User user, string password)
        {
            return passwordHasher.VerifyHashedPassword(user.Name, user.Password, password) != PasswordVerificationResult.Failed;
        }

        public enum ErrorType
        {
            InvalidInput,
            DuplicateEntry,
            InternalException
        }
    }
}