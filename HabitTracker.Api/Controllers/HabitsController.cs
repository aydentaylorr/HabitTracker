using HabitTracker.Api.Helpers;
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

    public HabitsController(
        IHabitRepository repo,
        HabitStatisticsService stats)
    {
        _repo = repo;
        _stats = stats;
    }

    private Guid CurrentUserId =>
        JwtHelper.GetUserId(User);

    [HttpGet]
    public async Task<IActionResult> GetHabits()
    {
        var habits = await _repo.GetUserHabits(CurrentUserId);
        return Ok(habits);
    }

    [HttpPost]
    public async Task<IActionResult> CreateHabit(Habit habit)
    {
        habit.UserId = CurrentUserId;

        var created = await _repo.AddHabit(habit);
        return Ok(created);
    }

    [HttpPost("{id}/complete")]
    public async Task<IActionResult> Complete(Guid id)
    {
        await _repo.CompleteHabit(
            id,
            DateOnly.FromDateTime(DateTime.Today));

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _repo.DeleteHabit(id, CurrentUserId);
        return NoContent();
    }

    // ⭐ NEW ENDPOINT
    [HttpGet("{id}/stats")]
    public async Task<IActionResult> GetStats(Guid id)
    {
        var stats = await _stats.GetStats(id, CurrentUserId);
        return Ok(stats);
    }
}