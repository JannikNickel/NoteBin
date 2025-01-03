using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;

namespace NoteBin
{
    public class StatelessTokenGen
    {
        private readonly byte[] secretKey;
        private readonly int tokenLength;
        private static readonly Encoding encoding = Encoding.UTF8;

        public StatelessTokenGen(byte[] secretKey, int tokenLength)
        {
            this.secretKey = secretKey;
            this.tokenLength = tokenLength;
        }

        public static byte[] GenerateSecretKey(int length)
        {
            return RandomNumberGenerator.GetBytes(length);
        }

        public string Generate(string user, TimeSpan expiration)
        {
            Span<byte> randomBytes = stackalloc byte[tokenLength];
            RandomNumberGenerator.Fill(randomBytes);

            string token = Convert.ToBase64String(randomBytes);
            long expirationTime = DateTimeOffset.UtcNow.Add(expiration).ToUnixTimeSeconds();

            string data = $"{user}.{token}.{expirationTime}";
            string encodedData = Convert.ToBase64String(encoding.GetBytes(data));
            string signature = CreateHmacSignature(encodedData);
            return $"{encodedData}.{signature}";
        }

        public bool Validate(string? token, [NotNullWhen(true)] out string? username)
        {
            username = null;
            if(string.IsNullOrEmpty(token))
            {
                return false;
            }

            string[] parts = token.Split('.');
            if(parts.Length != 2)
            {
                return false;
            }

            string encodedData = parts[0];
            string signature = parts[1];
            string expected = CreateHmacSignature(encodedData);
            if(!FixedTimeEquals(signature, expected))
            {
                return false;
            }

            string payload = encoding.GetString(Convert.FromBase64String(encodedData));
            string[] payloadData = payload.Split('.');
            if(payloadData.Length != 3 || !long.TryParse(payloadData[2], out long expirationTime))
            {
                return false;
            }

            username = payloadData[0];
            long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            return currentTime <= expirationTime;
        }

        private string CreateHmacSignature(string data)
        {
            using HMACSHA512 hmac = new HMACSHA512(secretKey);
            byte[] hash = hmac.ComputeHash(encoding.GetBytes(data));
            return Convert.ToBase64String(hash);
        }

        private static bool FixedTimeEquals(string a, string b)
        {
            int diff = a.Length ^ b.Length;
            for(int i = 0;i < a.Length && i < b.Length;i++)
            {
                diff |= a[i] ^ b[i];
            }
            return diff == 0;
        }
    }
}