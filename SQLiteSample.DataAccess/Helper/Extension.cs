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
                            Key = reader.GetValue(reader.GetOrdinal("Key")).ToString(),
                            Value = reader.GetValue(reader.GetOrdinal("Value")).ToString()
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

        public static void Add(this string tableName, KeyValueModel model)
        {
            using (var con = GetConnection())
            {
                var command = con.CreateCommand();
                command.CommandText = $"INSERT INTO {tableName}(Key, Value) VALUES(@Key, @Value)";
                command.Parameters.AddWithValue("@Key", model.Key);
                command.Parameters.AddWithValue("@Value", model.Value);
                command.ExecuteNonQuery();
            }
        }

        public static void Update(this string tableName, KeyValueModel model)
        {
            using (var con = GetConnection())
            {
                var command = con.CreateCommand();
                command.CommandText = $"UPDATE {tableName} SET Value=@Value WHERE Key=@Key";
                command.Parameters.AddWithValue("@Key", model.Key);
                command.Parameters.AddWithValue("@Value", model.Value);
                command.ExecuteNonQuery();
            }
        }

        public static void Delete(this string tableName, string key)
        {
            using (var con = GetConnection())
            {
                var command = con.CreateCommand();
                command.CommandText = $"DELETE FROM {tableName} WHERE Key=@Key";
                command.Parameters.AddWithValue("@Key", key);
                command.ExecuteNonQuery();
            }
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