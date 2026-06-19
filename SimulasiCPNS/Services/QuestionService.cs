using SimulasiCPNS.Models;

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

        public async Task<List<CategoryDisplayItem>> GetCategoriesAsync()
        {
            var questions = await GetQuestionsAsync();
            return questions
                .Where(q => !string.IsNullOrWhiteSpace(q.Category))
                .GroupBy(q => q.Category)
                .Select(group =>
                {
                    var firstQuestion = group.First();

                    return new CategoryDisplayItem
                    {
                        Category = firstQuestion.Category,
                        CategoryIcon = firstQuestion.CategoryIcon,
                        SubCategory = firstQuestion.SubCategory,
                        SubCategoryIcon = firstQuestion.SubCategoryIcon,
                        Difficulty = firstQuestion.Difficulty
                    };
                })
                .ToList();
        }

        public async Task<Question?> GetFeaturedQuestionAsync()
        {
            var questions = await GetQuestionsAsync();
            return questions.FirstOrDefault();
        }
    }
}
