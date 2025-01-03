using System.Data.SQLite;
using System.Linq;

namespace NoteBin.Models.Sqlite
{
    public class DbInsertCmd : DbTableCmd
    {
        protected DbInsertCmd(SQLiteConnection connection, string tableName, string[] columnNames, object?[] columnBindings)
            : base(connection, tableName)
        {
            string columns = string.Join(",", columnNames);
            string[] parameterIds = columnBindings.Select((_, i) => $"@p{i}").ToArray();
            string parameters = string.Join(",", parameterIds);

            string command = $"INSERT INTO {tableName} ({columns}) VALUES ({parameters});";
            cmd.CommandText = command;
            foreach((string id, object? binding) in parameterIds.Zip(columnBindings))
            {
                cmd.Parameters.AddWithValue(id, binding);
            }
        }
    }
}
