using HabitTracker.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace HabitTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly HabitDbContext _db;

    public TestController(HabitDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public IActionResult TestDb()
    {
        var canConnect = _db.Database.CanConnect();
        return Ok(new { databaseConnected = canConnect });
    }
}