namespace HabitTracker.Core.Models
{
    public class HabitCompletion
    {
        public Guid CompletionId { get; set; }
        public Guid HabitId { get; set; }
        public DateOnly CompletedDate { get; set; }
        public Habit Habit { get; set; } = null!;
    }
}
