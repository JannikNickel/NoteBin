using System.Data.SQLite;

namespace NoteBin.Models.Sqlite
{
    public abstract class DbTableCmd : DbCmd
    {
        protected readonly string tableName;

        protected DbTableCmd(SQLiteConnection connection, string tableName) : base(connection)
        {
            this.tableName = tableName;
        }
    }
}
