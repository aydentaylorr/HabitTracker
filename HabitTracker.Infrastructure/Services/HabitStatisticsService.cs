using HabitTracker.Core.DTOs.Habits;
using HabitTracker.Core.Interfaces;

namespace HabitTracker.Infrastructure.Services;

public class HabitStatisticsService
{
    private readonly IHabitRepository _repo;

    public HabitStatisticsService(IHabitRepository repo)
    {
        _repo = repo;
    }

    public async Task<HabitStatsDto> GetStats(Guid habitId, Guid userId)
    {
        var habit = await _repo.GetUserHabits(userId);
        var target = habit.FirstOrDefault(h => h.HabitId == habitId);

        if (target == null)
            return new HabitStatsDto();

        var dates = target.Completions
            .Select(c => c.CompletedDate)
            .ToList();

        return new HabitStatsDto
        {
            CurrentStreak = CalcCurrentStreak(dates),
            LongestStreak = CalcLongestStreak(dates),
            CompletionRate30Days = CalcRate30Days(dates),
            WeeklyData = CalcWeeklyData(dates)
        };
    }

    private static int CalcCurrentStreak(List<DateOnly> dates)
    {
        int streak = 0;
        var day = DateOnly.FromDateTime(DateTime.Today);

        while (dates.Contains(day))
        {
            streak++;
            day = day.AddDays(-1);
        }

        return streak;
    }

    private static int CalcLongestStreak(List<DateOnly> dates)
    {
        if (!dates.Any()) return 0;

        var sorted = dates.OrderBy(d => d).ToList();

        int longest = 1;
        int current = 1;

        for (int i = 1; i < sorted.Count; i++)
        {
            if (sorted[i] == sorted[i - 1].AddDays(1))
                current++;
            else
                current = 1;

            longest = Math.Max(longest, current);
        }

        return longest;
    }

    private static double CalcRate30Days(List<DateOnly> dates)
    {
        var cutoff = DateOnly.FromDateTime(DateTime.Today.AddDays(-29));

        var count = dates.Count(d => d >= cutoff);

        return Math.Round(count / 30.0 * 100, 1);
    }

    private static List<WeeklyDataPoint> CalcWeeklyData(List<DateOnly> dates)
    {
        return Enumerable.Range(0, 7).Select(i =>
        {
            var day = DateOnly.FromDateTime(DateTime.Today.AddDays(-6 + i));

            return new WeeklyDataPoint
            {
                Day = day.DayOfWeek.ToString()[..3],
                Count = dates.Contains(day) ? 1 : 0
            };
        }).ToList();
    }
}