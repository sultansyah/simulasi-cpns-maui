using SimulasiCPNS.Models;
using SQLite;
using System.Text.Json;

namespace SimulasiCPNS.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection? _database;

        public async Task<SQLiteAsyncConnection> GetDatabaseAsync()
        {
            if (_database is not null) return _database;

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "cpns-simulation-db.db3");

            _database = new SQLiteAsyncConnection(dbPath);

            await _database.CreateTableAsync<Question>();
            await _database.CreateTableAsync<Setting>();

            return _database;
        }

        public async Task SeedQuestionsAsync()
        {
            var database = await GetDatabaseAsync();

            var count = await database.Table<Question>().CountAsync();
            if (count > 0) return;

            using var stream = await FileSystem.OpenAppPackageFileAsync("questions.json");
            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();

            var questions = JsonSerializer.Deserialize<List<Question>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (questions is null || questions.Count == 0) return;

            foreach (var question in questions)
            {
                question.CreatedAt = DateTime.Now;
            }

            await database.InsertAllAsync(questions);
        }
    }
}
