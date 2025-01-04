using System;
using System.Security.Cryptography;

namespace NoteBin.Services
{
    public class RngNoteIdGenService : INoteIdGenService
    {
        private const int Length = Constants.NoteIdLength;
        private const string Base62Chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public string GenerateId()
        {
            Span<byte> guid = stackalloc byte[16];
            RandomNumberGenerator.Fill(guid);

            Span<char> id = stackalloc char[Length];
            for(int i = 0;i < Length;i++)
            {
                id[i] = Base62Chars[guid[i % guid.Length] % Base62Chars.Length];
            }
            return new string(id);
        }
    }
}