using SimulasiCPNS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimulasiCPNS.Services
{
    public class QuestionService(DatabaseService databaseService)
    {
        private readonly DatabaseService _databaseService = databaseService;

        public async Task<List<Question>> GetQuestionsAsync()
        {
            var database = await _databaseService.GetDatabaseAsync();
            return await database.Table<Question>().ToListAsync();
        }

        public async Task<List<string>> GetCategoriesAsync()
        {
            var questions = await GetQuestionsAsync();
            return questions
                    .Select(q => q.Category)
                    .Where(c => !string.IsNullOrWhiteSpace(c))
                    .Distinct()
                    .ToList();
        }
    }
}
