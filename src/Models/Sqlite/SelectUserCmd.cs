using System.Data.Common;
using System.Data.SQLite;

namespace NoteBin.Models.Sqlite
{
    public class SelectUserCmd : DbSelectCmd<User>
    {
        private readonly string name;

        public SelectUserCmd(SQLiteConnection connection, string name)
            : base(connection, CreateUserTableCmd.TableName, CreateUserTableCmd.Columns)
        {
            this.name = name;
            BuildCommand();
        }

        protected override string BuildFilter()
        {
            cmd.Parameters.AddWithValue("@username", name);
            return $"WHERE {CreateUserTableCmd.NameColumn} = @username";
        }

        protected override User ParseDataRow(DbDataReader reader)
        {
            string name = reader.GetString(0);
            string password = reader.GetString(1);
            long creationTime = reader.GetInt64(2);
            return new User(name, password, TimeUtils.FromUnixTimeMilliseconds(creationTime));
        }
    }
}
