using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NoteBin
{
    public static class SqLiteHelper
    {
        public static readonly string[] dataSourceKeys = 
        {
            "Data Source",
            "DataSource",
            "Filename"
        };

        public static SQLiteConnection Open(string connStr)
        {
            SQLiteConnection conn = new SQLiteConnection(connStr);
            conn.Open();
            return conn;
        }

        public static async Task<SQLiteConnection> OpenAsync(string connStr)
        {
            SQLiteConnection conn = new SQLiteConnection(connStr);
            await conn.OpenAsync();
            return conn;
        }

        public static void EnsureDataDirectory(string connectionString)
        {
            string? dataSource = FindDataSource(connectionString);
            if(dataSource != null)
            {
                string? directory = Path.GetDirectoryName(dataSource);
                if(directory != null)
                {
                    Directory.CreateDirectory(directory);
                }
            }
        }

        public static IEnumerable<(string key, string value)> ReadConnectionString(string connStr)
        {
            string[] parameters = connStr.Split(';', StringSplitOptions.RemoveEmptyEntries);
            foreach(string parameter in parameters)
            {
                string[] pair = parameter.Split('=', 2);
                if(pair.Length == 2)
                {
                    yield return (pair[0].Trim(), pair[1].Trim());
                }
            }
        }

        public static string? FindDataSource(string connStr)
        {
            foreach((string key, string value) in ReadConnectionString(connStr))
            {
                if(dataSourceKeys.Any(n => key.Equals(n, StringComparison.OrdinalIgnoreCase)))
                {
                    return value;
                }
            }
            return null;
        }
    }
}
