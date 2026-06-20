using SimulasiCPNS.Models;

namespace SimulasiCPNS.Services
{
    public class SettingService(DatabaseService databaseService)
    {
        private readonly DatabaseService _databaseService = databaseService;

        public async Task<Setting?> GetSettingAsync()
        {
            var database = await _databaseService.GetDatabaseAsync();
            return await database.Table<Setting>().FirstOrDefaultAsync();
        }

        public async Task<int> SaveSettingAsync(Setting setting)
        {
            var database = await _databaseService.GetDatabaseAsync();

            var existingSetting = await GetSettingAsync();
            if (existingSetting is null)
            {
                return await database.InsertAsync(setting);
            }

            setting.Id = existingSetting.Id;
            return await database.UpdateAsync(setting);
        }
    }
}
