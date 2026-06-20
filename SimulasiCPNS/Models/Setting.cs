using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimulasiCPNS.Models
{
    public class Setting
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string FullName { get; set; } = "";
        public bool ReminderEnabled { get; set; }
        public string ReminderTime { get; set; } = "";

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
