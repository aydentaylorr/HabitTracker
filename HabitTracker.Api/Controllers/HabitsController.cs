using HabitTracker.Api.Helpers;
using HabitTracker.Core.DTOs.Habits;
using HabitTracker.Core.Enums;
using HabitTracker.Core.Interfaces;
using HabitTracker.Core.Models;
using HabitTracker.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HabitTracker.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class HabitsController : ControllerBase
{
    private readonly IHabitRepository _repo;
    private readonly HabitStatisticsService _stats;

    public HabitsController(IHabitRepository repo, HabitStatisticsService stats)
    {
        _repo = repo;
        _stats = stats;
    }

    private Guid CurrentUserId => JwtHelper.GetUserId(User);

    [HttpGet]
    public async Task<IActionResult> GetHabits()
    {
        var habits = await _repo.GetUserHabits(CurrentUserId);
        var today = DateOnly.FromDateTime(DateTime.Today);

        var result = habits.Select(h => new HabitDto
        {
            HabitId = h.HabitId,
            Name = h.Name,
            Icon = h.Icon,
            CompletedToday = h.Completions.Any(c => c.CompletedDate == today),
            CompletionDates = h.Completions
                .Select(c => c.CompletedDate.ToString("yyyy-MM-dd"))
                .ToList()
        });

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateHabit(CreateHabitDto dto)
    {
        var habit = new Habit
        {
            HabitId = Guid.NewGuid(),
            UserId = CurrentUserId,
            Name = dto.Name,
            Icon = dto.Icon,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            Frequency = HabitFrequency.Daily
        };

        var created = await _repo.AddHabit(habit);

        return Ok(new HabitDto
        {
            HabitId = created.HabitId,
            Name = created.Name,
            Icon = created.Icon,
            CompletedToday = false,
            CompletionDates = new List<string>()
        });
    }

    [HttpPost("{id}/complete")]
    public async Task<IActionResult> Complete(Guid id)
    {
        await _repo.CompleteHabit(id, DateOnly.FromDateTime(DateTime.Today));
        return Ok();
    }

    [HttpDelete("{id}/complete")]
    public async Task<IActionResult> Uncomplete(Guid id)
    {
        await _repo.UncompleteHabit(id, DateOnly.FromDateTime(DateTime.Today));
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _repo.DeleteHabit(id, CurrentUserId);
        return NoContent();
    }

    [HttpGet("{id}/stats")]
    public async Task<IActionResult> GetStats(Guid id)
    {
        var stats = await _stats.GetStats(id, CurrentUserId);
        return Ok(stats);
    }
}