using HabitTracker.Api.Helpers;
using HabitTracker.Core.DTOs.Auth;
using HabitTracker.Core.Interfaces;
using HabitTracker.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace HabitTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthRepository _auth;
    private readonly JwtHelper _jwt;

    public AuthController(IAuthRepository auth, JwtHelper jwt)
    {
        _auth = auth;
        _jwt = jwt;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        if (await _auth.EmailExists(dto.Email))
            return BadRequest("Email already exists");

        var user = new User
        {
            Email = dto.Email,
            Username = dto.Username
        };

        var created = await _auth.Register(user, dto.Password);

        return Ok(new AuthResponseDto
        {
            Token = _jwt.GenerateToken(created),
            UserId = created.UserId,
            Username = created.Username
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _auth.Login(dto.Email, dto.Password);

        if (user == null)
            return Unauthorized();

        return Ok(new AuthResponseDto
        {
            Token = _jwt.GenerateToken(user),
            UserId = user.UserId,
            Username = user.Username
        });
    }
}