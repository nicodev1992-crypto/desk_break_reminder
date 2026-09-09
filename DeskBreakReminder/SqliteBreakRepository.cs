using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeskBreakReminder
{
    internal class SqliteBreakRepository : IBreakRepository
    {
        private readonly string connectionString;

        public SqliteBreakRepository(string connectionString = "Data Source=DeskBreaks.db")
        {
            this.connectionString = connectionString;
        }

        public async Task<float> GetBreakDuration(int breakID)
        {
            using var connection = new SqliteConnection(connectionString);
            await connection.OpenAsync();

            string sqlQuery = @"
                              SELECT b.Duration
                              FROM Breaks b
                              WHERE b.Id = @breakID;";

            using var command = new SqliteCommand(sqlQuery, connection);
            command.Parameters.AddWithValue("@breakID", breakID);
            var result = await command.ExecuteScalarAsync();
            return result != null ? Convert.ToSingle(result) : 0f;
        }

        public async Task<int> GetTodayBreakCountAsync(string breakType)
        {
            using var connection = new SqliteConnection(connectionString);

            await connection.OpenAsync();

            string today = DateTime.Now.ToString("yyyy-MM-dd");
            string selectQuery = @"
                SELECT COUNT(*) FROM Breaks
                WHERE BreakType = @type AND Timestamp LIKE @todayPattern;";

            using var command = new SqliteCommand(selectQuery, connection);
            command.Parameters.AddWithValue("@type", breakType);
            command.Parameters.AddWithValue("@todayPattern", today + "%");

            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task InitializeAsync()
        {
            using var connection = new SqliteConnection(connectionString);

            await connection.OpenAsync();

            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS Breaks (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    BreakType TEXT NOT NULL,
                    Timestamp TEXT NOT NULL,
                    Duration INTEGER DEFAULT 0
                );";

            using var command = new SqliteCommand(createTableQuery, connection);
            await command.ExecuteNonQueryAsync();
        }

        public async Task LogBreakAsync(string breakType)
        {
            using var connection = new SqliteConnection(connectionString);
            await connection.OpenAsync();

            string insertQuery = @"
                INSERT INTO Breaks (BreakType, Timestamp)
                VALUES (@type, @timestamp);";

            string today = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            using var command = new SqliteCommand(insertQuery, connection);
            command.Parameters.AddWithValue("@type", breakType);
            command.Parameters.AddWithValue("@timestamp", today);

            await command.ExecuteNonQueryAsync();
        }

        public async Task<List<BreakRecord>> GetTodayBreaksAsync()
        {
            using var connection = new SqliteConnection(connectionString);
            await connection.OpenAsync();

            string SQLcommand = @"SELECT id, BreakType, TimeSTamp FROM Breaks WHERE Timestamp LIKE @todayPattern;";

            using var command = new SqliteCommand(SQLcommand, connection);
            command.Parameters.AddWithValue("@todayPattern", DateTime.Now.ToString("yyyy-MM-dd") + "%");

            await using var reader = await command.ExecuteReaderAsync();

            var breakRecords = new List<BreakRecord>();

            while (await reader.ReadAsync())
            {
                var record = new BreakRecord
                {
                    Id = reader.GetInt32(0),
                    BreakType = reader.GetString(1),
                    Timestamp = DateTime.Parse(reader.GetString(2))
                };

                breakRecords.Add(record);
            }

            return breakRecords;
        }
    }
}
