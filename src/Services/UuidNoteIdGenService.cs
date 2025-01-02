using System;

namespace NoteBin.Services
{
    public class UuidNoteIdGenService : INoteIdGenService
    {
        private const int length = 10;
        private const string base62Chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public string GenerateId()
        {
            Span<byte> guid = stackalloc byte[16];
            Guid.NewGuid().TryWriteBytes(guid);

            Span<char> id = stackalloc char[length];
            for(int i = 0;i < length;i++)
            {
                id[i] = base62Chars[guid[i % guid.Length] % base62Chars.Length];
            }
            return new string(id);
        }
    }
}