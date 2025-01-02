using System;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace NoteBin.Models.Sqlite
{
    public abstract class DbTableCmd : DbCmd
    {
        protected readonly string tableName;

        protected DbTableCmd(SQLiteConnection connection, string tableName) : base(connection)
        {
            this.tableName = tableName;
        }

        public virtual async Task<SQLiteErrorCode> ExecuteAsync()
        {
            try
            {
                await cmd.ExecuteNonQueryAsync();
                return SQLiteErrorCode.Ok;
            }
            catch(SQLiteException e)
            {
                Console.WriteLine($"[{GetType()}] {nameof(ExecuteAsync)}: {e.Message}");
                return e.ResultCode;
            }
        }
    }
}
