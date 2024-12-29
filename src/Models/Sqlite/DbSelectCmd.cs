using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace NoteBin.Models.Sqlite
{
    public abstract class DbSelectCmd<T> : DbTableCmd
    {
        private readonly string[] columnNames;

        protected DbSelectCmd(SQLiteConnection connection, string tableName, string[] columnNames)
            : base(connection, tableName)
        {
            this.columnNames = columnNames;
        }

        protected void BuildCommand()
        {
            string columns = string.Join(",", columnNames);
            string command = $"SELECT {columns} FROM {tableName} WHERE {BuildFilter()};";
            cmd.CommandText = command;
        }

        protected abstract string BuildFilter();
        protected abstract T ParseDataRow(DbDataReader reader);

        public async Task<T?> ReadFirstRowAsync()
        {
            using(DbDataReader reader = await cmd.ExecuteReaderAsync())
            {
                if(await reader.ReadAsync())
                {
                    return ParseDataRow(reader);
                }
            }
            return default;
        }

        public async IAsyncEnumerable<T> ReadRowsAsync()
        {
            using(DbDataReader reader = await cmd.ExecuteReaderAsync())
            {
                while(await reader.ReadAsync())
                {
                    yield return ParseDataRow(reader);
                }
            }
        }
    }
}
