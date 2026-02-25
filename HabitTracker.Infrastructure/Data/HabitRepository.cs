using HabitTracker.Core.Interfaces;
using HabitTracker.Core.Models;
using HabitTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Infrastructure.Data
{
    public class HabitRepository : IHabitRepository
    {
        private readonly HabitDbContext _db;

        public HabitRepository(HabitDbContext db)
        {
            _db = db;
        }

        public async Task<List<Habit>> GetUserHabits(Guid userId)
        {
            return await _db.Habits
                .Where(h => h.UserId == userId && !h.IsDeleted)
                .Include(h => h.Completions)
                .ToListAsync();
        }

        public async Task<Habit> AddHabit(Habit habit)
        {
            _db.Habits.Add(habit);
            await _db.SaveChangesAsync();
            return habit;
        }

        public async Task CompleteHabit(Guid habitId, DateOnly date)
        {
            var exists = await _db.HabitCompletions
                .AnyAsync(x => x.HabitId == habitId && x.CompletedDate == date);

            if (!exists)
            {
                _db.HabitCompletions.Add(new HabitCompletion
                {
                    HabitId = habitId,
                    CompletedDate = date
                });

                await _db.SaveChangesAsync();
            }
        }

        public async Task DeleteHabit(Guid habitId, Guid userId)
        {
            var habit = await _db.Habits
                .FirstOrDefaultAsync(h => h.HabitId == habitId && h.UserId == userId);

            if (habit != null)
            {
                habit.IsDeleted = true;
                await _db.SaveChangesAsync();
            }
        }
    }
}
