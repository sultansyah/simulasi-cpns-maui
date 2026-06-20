using SQLite;
using System;

namespace SimulasiCPNS.Models
{
    public class BookmarkedQuestion
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int QuestionId { get; set; }
        public string Type { get; set; } = ""; // bookmark / review_later / wrong
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
