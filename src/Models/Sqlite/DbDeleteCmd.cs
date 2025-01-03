using System.Data.SQLite;

namespace NoteBin.Models.Sqlite
{
    public abstract class DbDeleteCmd : DbTableCmd
    {
        protected DbDeleteCmd(SQLiteConnection connection, string tableName) : base(connection, tableName) { }

        protected void BuildCommand()
        {
            string command = $"DELETE FROM {tableName} WHERE {BuildFilter()};";
            cmd.CommandText = command;
        }

        protected abstract string BuildFilter();
    }
}
