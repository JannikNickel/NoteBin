using System.Data.SQLite;

namespace NoteBin.Models.Sqlite
{
    public class InsertUserCmd : DbInsertCmd
    {
        public InsertUserCmd(SQLiteConnection connection, User user)
            : base(connection, CreateUserTableCmd.TableName, CreateUserTableCmd.Columns, GetColumnBindings(user))
        {

        }

        private static object[] GetColumnBindings(User user) => [user.Name, user.Password, TimeUtils.ToUnixTimeMilliseconds(user.CreationTime)];
    }
}