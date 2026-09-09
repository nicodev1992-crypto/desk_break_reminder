using System;
using System.Collections.Generic;
using System.Text;

namespace DeskBreakReminder
{
    internal interface IBreakRepository
    {
        Task InitializeAsync();
        Task LogBreakAsync(string breakType);
        Task<int> GetTodayBreakCountAsync(string breakType);
        Task<List<BreakRecord>> GetTodayBreaksAsync();
        Task<float> GetBreakDuration(int breakID);
    }

    public class BreakRecord
    {
        public int Id { get; set; }
        public string BreakType { get; set; } // es. "Eyes" o "Movement"
        public System.DateTime Timestamp { get; set; }

        public float Duration { get; set; } // Durata della pausa in minuti
    }
}
