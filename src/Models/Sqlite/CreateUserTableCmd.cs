using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace NoteBin.Models.Sqlite
{
    public class CreateUserTableCmd : DbCreateTableCmd
    {
        public const string TableName = "users";
        public const string NameColumn = "username";
        public const string PasswordColumn = "password";
        public const string CreationTimeColumn = "creation_time";

        public static string[] Columns => [NameColumn, PasswordColumn, CreationTimeColumn];

        public CreateUserTableCmd(SQLiteConnection connection) : base(connection, TableName)
        {

        }

        protected override IEnumerable<string> GenerateColumns()
        {
            yield return $"{NameColumn} TEXT NOT NULL PRIMARY KEY";
            yield return $"{PasswordColumn} TEXT NOT NULL";
            yield return $"{CreationTimeColumn} INTEGER NOT NULL";
        }

        public void Execute() => cmd.ExecuteNonQuery();
    }
}
