using HabitTracker.Core.Enums;

namespace HabitTracker.Core.Models;

public class Habit
{
    public Guid HabitId { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = "💪";
    public HabitFrequency Frequency { get; set; } = HabitFrequency.Daily;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;

    public User User { get; set; } = null!;
    public ICollection<HabitCompletion> Completions { get; set; } = new List<HabitCompletion>();
}