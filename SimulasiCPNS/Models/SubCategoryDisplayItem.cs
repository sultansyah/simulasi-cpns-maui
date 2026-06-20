using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SimulasiCPNS.Models
{
    public class SubCategoryDisplayItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private bool _isSelected;

        public string SubCategory { get; set; } = "";
        public string SubCategoryIcon { get; set; } = "";
        public string Difficulty { get; set; } = "";
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value)
                    return;

                _isSelected = value;

                PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs(nameof(IsSelected)));

                PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs(nameof(BackgroundColor)));

                PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs(nameof(TextColor)));
            }
        }
        public Color BackgroundColor =>
            IsSelected ? Color.FromArgb("#E11D48") : Colors.White;
        public Color TextColor =>
            IsSelected ? Colors.White : Colors.Black;
    }
}
