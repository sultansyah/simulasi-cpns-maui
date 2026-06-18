using SimulasiCPNS.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
