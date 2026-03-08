using ECommerceApi.Data;
using ECommerceApi.DTOs.Auth;
using ECommerceApi.Models;
using ECommerceApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly ITokenService _tokenService;
    private readonly IEmailService _emailService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        AppDbContext db,
        ITokenService tokenService,
        IEmailService emailService,
        ILogger<AuthController> logger)
    {
        _db = db;
        _tokenService = tokenService;
        _emailService = emailService;
        _logger = logger;
    }

    /// <summary>Register a new customer account.</summary>
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto dto)
    {
        if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
            return Conflict(new { message = "Email is already registered." });

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        // Fire-and-forget welcome email — failure is logged but does not block registration
        _ = _emailService.SendWelcomeEmailAsync(user.Email, user.Name)
            .ContinueWith(t => _logger.LogWarning(t.Exception, "Welcome email failed for {Email}", user.Email),
                TaskContinuationOptions.OnlyOnFaulted);

        var token = _tokenService.CreateToken(user);
        return Ok(new AuthResponseDto { Token = token, Email = user.Email, Name = user.Name, Role = user.Role });
    }

    /// <summary>Login and receive a JWT bearer token.</summary>
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
    {
        var user = await _db.Users.SingleOrDefaultAsync(u => u.Email == dto.Email);

        if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return Unauthorized(new { message = "Invalid email or password." });

        var token = _tokenService.CreateToken(user);
        return Ok(new AuthResponseDto { Token = token, Email = user.Email, Name = user.Name, Role = user.Role });
    }
}
