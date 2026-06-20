using SQLite;
using System.ComponentModel;

namespace SimulasiCPNS.Models
{
    public class Question : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Category { get; set; } = "";
        public string CategoryIcon { get; set; } = "";
        public string SubCategory{ get; set; } = "";
        public string SubCategoryIcon { get; set; } = "";
        public string Difficulty { get; set; } = "";
        public string QuestionText { get; set; } = "";

        public string OptionA { get; set; } = "";
        public string OptionB { get; set; } = "";
        public string OptionC { get; set; } = "";
        public string OptionD { get; set; } = "";
        public string OptionE { get; set; } = "";

        public string CorrectAnswer { get; set; } = "";
        public string Explanation { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Ignore]
        public string DifficultyDisplay => string.IsNullOrWhiteSpace(Difficulty) ? "" : char.ToUpper(Difficulty[0]) + Difficulty[1..];
        [Ignore]
        public int DisplayNumber { get; set; }

        private bool _isBookmarked;
        [Ignore]
        public bool IsBookmarked
        {
            get => _isBookmarked;
            set
            {
                if (_isBookmarked == value) return;
                _isBookmarked = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsBookmarked)));
            }
        }
    }
}
