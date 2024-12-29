using System;
using System.Data.SQLite;

namespace NoteBin.Models.Sqlite
{
	public abstract class DbCmd : IDisposable
	{
		protected SQLiteConnection connection;
        protected SQLiteCommand cmd;

        protected DbCmd(SQLiteConnection connection)
        {
            this.connection = connection;
            cmd = connection.CreateCommand();
        }

        protected void NextCommand()
        {
            cmd.Dispose();
            cmd = connection.CreateCommand();
        }

        public virtual void Dispose()
        {
            cmd.Dispose();
        }
    }
}
