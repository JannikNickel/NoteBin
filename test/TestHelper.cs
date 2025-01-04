using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NoteBin.Configuration;
using NoteBin.Models;
using NoteBin.Models.API;
using NoteBin.Services;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace NoteBin.Test
{
    public static class TestHelper
    {
        public static AuthSettings AuthSettings => new AuthSettings
        {
            AuthType = AuthType.Stateless,
            KeyFile = TempKeyFile(),
            KeyLength = 64,
            TokenLength = 64,
            ExpirationDuration = int.MaxValue
        };

        public static string TempConnectionSource(out string tmpFile)
        {
            tmpFile = Path.GetTempFileName();
            return $"Data Source={tmpFile}";
        }

        public static string TempKeyFile() => Path.GetTempFileName();
        public static string TempContentDirectory() => Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        [return: NotNullIfNotNull(nameof(user))]
        public static User? FakeUser(string? user) => user != null ? new User(user, "ValidPassword123", DateTime.UtcNow) : null;
        public static string FakeNoteId() => Services.IdGen.GenerateId();

        public static NoteCreateRequest NoteCreateRequest(string? name, string? fork, string syntax, string content) => new NoteCreateRequest()
        {
            Name = name,
            Fork = fork,
            Syntax = syntax,
            Content = content
        };

        public static ControllerContext HttpContextForToken(string? token)
        {
            DefaultHttpContext httpContext = new DefaultHttpContext();
            httpContext.Request.Headers.Authorization = $"Bearer {token}";
            return new ControllerContext()
            {
                HttpContext = httpContext
            };
        }

        public static ControllerContext EmptyHttpContext() => new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
        };

        public static class Services
        {
            public static INoteIdGenService IdGen => new RngNoteIdGenService();
            public static INoteContentService NoteContent => new MemoryNoteContentService();
            public static IUserDbService UserDb => new MemoryUserDbService();
            public static INoteDbService NoteDb => new MemoryNoteDbService(IdGen, NoteContent);
            public static IAuthService Auth(IUserDbService userService) => new StatelessAuthService(AuthSettings, userService);
        }
    }
}
