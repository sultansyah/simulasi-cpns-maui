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
            var categories = questions
                .Where(q => !string.IsNullOrWhiteSpace(q.Category))
                .GroupBy(q => q.Category)
                .Select(group =>
                {
                    var firstQuestion = group.First();

                    return new CategoryDisplayItem
                    {
                        Category = firstQuestion.Category,
                        CategoryIcon = firstQuestion.CategoryIcon,
                        Description = GetCategoryDescription(firstQuestion.Category),
                        SubCategory = firstQuestion.SubCategory,
                        SubCategoryIcon = firstQuestion.SubCategoryIcon,
                        Difficulty = firstQuestion.Difficulty
                    };
                })
                .OrderBy(item => GetCategoryOrder(item.Category))
                .ToList();

            categories.Add(new CategoryDisplayItem
            {
                Category = "Mixed",
                CategoryIcon = "🗂️",
                Description = "Campuran semua kategori"
            });

            return categories;
        }

        public async Task<Question?> GetFeaturedQuestionAsync()
        {
            var questions = await GetQuestionsAsync();
            return questions.FirstOrDefault();
        }

        private static string GetCategoryDescription(string category) => category switch
        {
            "TWK" => "Tes Wawasan Kebangsaan",
            "TIU" => "Tes Intelegensi Umum",
            "TKP" => "Tes Karakteristik Pribadi",
            "Mixed" => "Campuran semua kategori",
            _ => "Latihan soal sesuai kategori"
        };

        private static int GetCategoryOrder(string category) => category switch
        {
            "TWK" => 1,
            "TIU" => 2,
            "TKP" => 3,
            "Mixed" => 4,
            _ => 99
        };
    }
}
