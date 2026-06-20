using SimulasiCPNS.Enums;
using SimulasiCPNS.Models;

namespace SimulasiCPNS.Services
{
    public class BookmarkedService(DatabaseService databaseService, QuestionService questionService)
    {
        private readonly DatabaseService _databaseService = databaseService;
        private readonly QuestionService _questionService = questionService;

        public async Task<List<BookmarkedQuestion>> GetBookmarkedAsync()
        {
            var database = await _databaseService.GetDatabaseAsync();
            return await database.Table<BookmarkedQuestion>().ToListAsync();
        }

        public async Task<BookmarkedQuestion?> GetByQuestionIdAsync(int questionId)
        {
            var bookmarks = await GetBookmarkedAsync();
            return bookmarks.Where(q => q.QuestionId == questionId).FirstOrDefault();
        }

        public async Task<int> Insert(int questionId, BookmarkedQuestionType type)
        {
            var database = await _databaseService.GetDatabaseAsync();
            return await database.InsertAsync(new BookmarkedQuestion
            {
                QuestionId = questionId,
                Type = type.ToString()
            });
        }

        public async Task<int> Delete(BookmarkedQuestion bookmarkedQuestion)
        {
            var database = await _databaseService.GetDatabaseAsync();
            return await database.DeleteAsync(bookmarkedQuestion);
        }

        public async Task<int> Upsert(int questionId, BookmarkedQuestionType type)
        {
            var database = await _databaseService.GetDatabaseAsync();

            // just delete it if its found and same type, if not insert
            var isBookmarkExist = await GetByQuestionIdAsync(questionId);
            if (isBookmarkExist is null) return await database.InsertAsync(new BookmarkedQuestion
                {
                    QuestionId = questionId,
                    Type = type.ToString()
                });

            if (isBookmarkExist.Type == type.ToString()) return await database.DeleteAsync(isBookmarkExist);
            else return await database.InsertAsync(new BookmarkedQuestion
                {
                    QuestionId = questionId,
                    Type = type.ToString()
                });
        }
    }
}
