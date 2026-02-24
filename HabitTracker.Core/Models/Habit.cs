namespace HabitTracker.Core.Models
{
    public class Habit
    {
        public Guid HabitId { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = "💪";
        public bool IsDeleted { get; set; }
        public User User { get; set; } = null!;
        public ICollection<HabitCompletion> Completions { get; set; }
            = new List<HabitCompletion>();
    }
}
