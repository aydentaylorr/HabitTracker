using HabitTracker.Core.Models;

namespace HabitTracker.Core.Interfaces
{
    public interface IHabitRepository
    {
        Task<List<Habit>> GetUserHabits(Guid userId);
        Task<Habit> AddHabit(Habit habit);
        Task CompleteHabit(Guid habitId, DateOnly date);
        Task DeleteHabit(Guid habitId, Guid userId);
    }
}
