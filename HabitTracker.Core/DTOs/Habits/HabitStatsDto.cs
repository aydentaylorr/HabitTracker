namespace HabitTracker.Core.DTOs.Habits
{
    public class HabitStatsDto
    {
        public int CurrentStreak { get; set; }
        public int LongestStreak { get; set; }
        public double CompletionRate30Days { get; set; }

        public List<WeeklyDataPoint> WeeklyData { get; set; } = new();
    }

    public class WeeklyDataPoint
    {
        public string Day { get; set; } = "";
        public int Count { get; set; }
    }
}
