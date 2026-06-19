using SQLite;
using System;

namespace SimulasiCPNS.Models
{
    public class QuizAnswer
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int SessionId { get; set; }
        public int QuestionId { get; set; }
        public string SelectedAnswer { get; set; } = "";
        public bool IsCorrect { get; set; }
        public int TimeSpentSeconds { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
