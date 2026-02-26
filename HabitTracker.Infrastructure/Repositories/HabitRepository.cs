using HabitTracker.Core.Interfaces;
using HabitTracker.Core.Models;
using HabitTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Infrastructure.Repositories;

public class HabitRepository : IHabitRepository
{
    private readonly HabitDbContext _db;

    public HabitRepository(HabitDbContext db) => _db = db;

    public async Task<List<Habit>> GetUserHabits(Guid userId)
        => await _db.Habits
            .Where(h => h.UserId == userId)
            .Include(h => h.Completions)
            .OrderBy(h => h.CreatedAt)
            .ToListAsync();

    public async Task<Habit?> GetHabitById(Guid habitId, Guid userId)
        => await _db.Habits
            .Include(h => h.Completions)
            .FirstOrDefaultAsync(h => h.HabitId == habitId && h.UserId == userId);

    public async Task<Habit> AddHabit(Habit habit)
    {
        _db.Habits.Add(habit);
        await _db.SaveChangesAsync();
        return habit;
    }

    public async Task DeleteHabit(Guid habitId, Guid userId)
    {
        var habit = await _db.Habits
            .FirstOrDefaultAsync(h => h.HabitId == habitId && h.UserId == userId);

        if (habit != null)
        {
            habit.IsDeleted = true; // soft delete — keeps history intact
            await _db.SaveChangesAsync();
        }
    }

    public async Task CompleteHabit(Guid habitId, DateOnly date)
    {
        var alreadyDone = await _db.HabitCompletions
            .AnyAsync(c => c.HabitId == habitId && c.CompletedDate == date);

        if (!alreadyDone)
        {
            _db.HabitCompletions.Add(new HabitCompletion
            {
                HabitId = habitId,
                CompletedDate = date
            });
            await _db.SaveChangesAsync();
        }
    }

    public async Task UncompleteHabit(Guid habitId, DateOnly date)
    {
        var completion = await _db.HabitCompletions
            .FirstOrDefaultAsync(c => c.HabitId == habitId && c.CompletedDate == date);

        if (completion != null)
        {
            _db.HabitCompletions.Remove(completion);
            await _db.SaveChangesAsync();
        }
    }
}