using SQLite;
using System;

namespace SimulasiCPNS.Models
{
    public class QuizSession
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Mode { get; set; } = "";
        public string Category { get; set; } = "";
        public int TotalQuestions { get; set; }
        public int CorrectCount { get; set; }
        public int WrongCount { get; set; }
        public double Score { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
    }
}
