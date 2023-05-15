using Microsoft.Data.Sqlite;
using SQLiteSample.Data.Entitiy;
using System.Collections.Generic;

namespace SQLiteSample.DataAccess.Helper
{
    public static class Extension
    {
        private static SqliteConnection GetConnection()
        {
            var connection = new SqliteConnection("Data Source=slhkl.db");
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_winsqlite3());
            connection.Open();
            return connection;
        }

        public static List<KeyValueModel> Get(this string tableName)
        {
            List<KeyValueModel> list = new List<KeyValueModel>();
            using (var con = GetConnection())
            {
                var command = con.CreateCommand();
                command.CommandText = $"SELECT * FROM {tableName}";
                using (var reader = command.ExecuteReader())
                    while (reader.Read())
                        list.Add(new KeyValueModel()
                        {
                            Key = reader.GetOrdinal("key").ToString(),
                            Value = reader.GetOrdinal("value").ToString()
                        });
            }
            return list;
        }

        public static string Get(this string tableName, string key)
        {
            string value = null;
            using (var con = GetConnection())
            {
                var command = con.CreateCommand();
                command.CommandText = $"SELECT * FROM {tableName} WHERE Key=@Key";
                command.Parameters.AddWithValue("@Key", key);
                using (var reader = command.ExecuteReader())
                    while (reader.Read())
                        value = reader.GetValue(reader.GetOrdinal("Value")).ToString();
            }
            return value;
        }

        public static void CreateTable(this string tableName)
        {
            using (var con = GetConnection())
            {
                var command = con.CreateCommand();
                command.CommandText = $"CREATE TABLE IF NOT EXISTS [{tableName}] (" +
                   "[Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," +
                   "[Key] TEXT," +
                   "[Value] TEXT" +
                   ");";

                command.ExecuteNonQuery();
            }
        }
    }
}