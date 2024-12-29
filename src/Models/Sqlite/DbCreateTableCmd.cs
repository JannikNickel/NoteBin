using System.Collections.Generic;
using System.Data.SQLite;

namespace NoteBin.Models.Sqlite
{
    public abstract class DbCreateTableCmd : DbTableCmd
    {
        protected DbCreateTableCmd(SQLiteConnection connection, string tableName) : base(connection, tableName)
        {
            string columnDefs = string.Join(",", GenerateColumns());
            string command = $"CREATE TABLE IF NOT EXISTS {tableName} ({columnDefs});";
            cmd.CommandText = command;
        }

        protected abstract IEnumerable<string> GenerateColumns();
    }
}
