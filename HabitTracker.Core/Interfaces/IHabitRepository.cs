using HabitTracker.Core.Models;

namespace HabitTracker.Core.Interfaces
{
    public interface IHabitRepository
    {
        Task<List<Habit>> GetUserHabits(Guid userId);
        Task<Habit?> GetHabitById(Guid habitId, Guid userId);
        Task<Habit> AddHabit(Habit habit);
        Task DeleteHabit(Guid habitId, Guid userId);
        Task CompleteHabit(Guid habitId, DateOnly date);
        Task UncompleteHabit(Guid habitId, DateOnly date);
    }
}
