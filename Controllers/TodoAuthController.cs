using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TodosApi.Data;

namespace TodosApi.Controllers;

[ApiController]
[Route("api/auth")]
public class TodoAuthController : ControllerBase
{
    private readonly TodoDbContext _db;
    private readonly IConfiguration _config;
    private readonly ILogger<TodoAuthController> _logger;

    public TodoAuthController(TodoDbContext db, IConfiguration config, ILogger<TodoAuthController> logger)
    {
        _db = db;
        _config = config;
        _logger = logger;
    }

    /// <summary>Creates a new user account</summary>
    /// <param name="dto">Username (3-100 chars) and password (min 8 chars)</param>
    /// <returns>201 with user id and username, 409 if username taken, 400 if validation fails</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var username = dto.Username.Trim().ToLowerInvariant();

        if (await _db.Users.AnyAsync(u => u.Username == username))
            return Conflict(new ErrorResponse("Username already taken"));

        CreatePasswordHash(dto.Password, out var hash, out var salt);
        var user = new User
        {
            Username = username,
            PasswordHash = hash,
            PasswordSalt = salt
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        _logger.LogInformation("User registered: {Username}", username);

        return Created($"/api/auth/user/{user.Id}", new RegisterResponse(
            user.Id,
            user.Username,
            "User registered successfully"
        ));
    }

    /// <summary>Authenticates a user and returns a JWT token</summary>
    /// <param name="dto">Username and password</param>
    /// <returns>200 with JWT token and user info, 401 if credentials are invalid</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var username = dto.Username.Trim().ToLowerInvariant();

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null)
            return Unauthorized(new ErrorResponse("Invalid credentials"));

        if (!VerifyPassword(dto.Password, user.PasswordHash, user.PasswordSalt))
            return Unauthorized(new ErrorResponse("Invalid credentials"));

        var token = GenerateToken(user);

        _logger.LogInformation("User logged in: {Username}", username);

        return Ok(new AuthResponse(
            token,
            new UserResponse(user.Id, user.Username),
            "Login successful"
        ));
    }

    private string GenerateToken(User user)
    {
        var jwt = _config.GetSection("Jwt");
        var key = jwt.GetValue<string>("Key")!;
        var issuer = jwt.GetValue<string>("Issuer");
        var audience = jwt.GetValue<string>("Audience");
        var expiresMinutes = jwt.GetValue<int>("ExpiresMinutes");

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var creds = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiresMinutes > 0 ? expiresMinutes : 60),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static void CreatePasswordHash(string password, out string hash, out string salt)
    {
        using var rng = RandomNumberGenerator.Create();
        var saltBytes = new byte[16];
        rng.GetBytes(saltBytes);
        salt = Convert.ToBase64String(saltBytes);

        using var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 100_000, HashAlgorithmName.SHA256);
        hash = Convert.ToBase64String(pbkdf2.GetBytes(32));
    }

    private static bool VerifyPassword(string password, string expectedHash, string salt)
    {
        var saltBytes = Convert.FromBase64String(salt);
        using var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 100_000, HashAlgorithmName.SHA256);
        var hash = Convert.ToBase64String(pbkdf2.GetBytes(32));
        return CryptographicOperations.FixedTimeEquals(
            Convert.FromBase64String(expectedHash),
            Convert.FromBase64String(hash));
    }
}
