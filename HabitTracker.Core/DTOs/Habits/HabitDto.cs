namespace HabitTracker.Core.DTOs.Habits;

public class HabitDto
{
    public Guid HabitId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public bool CompletedToday { get; set; }
    public List<string> CompletionDates { get; set; } = new();
}