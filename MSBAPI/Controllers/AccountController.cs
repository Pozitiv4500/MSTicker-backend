using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MSBAPI.DTO;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtTokenGenerator _tokenGenerator;

    public AccountController(UserManager<IdentityUser> userManager, JwtTokenGenerator tokenGenerator)
    {
        _userManager = userManager;
        _tokenGenerator = tokenGenerator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationDto userDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingUser = await _userManager.FindByEmailAsync(userDto.Email);
        if (existingUser != null)
            return Conflict("User already exists.");

        var user = new IdentityUser
        {
            UserName = userDto.Email,
            Email = userDto.Email
        };

        var result = await _userManager.CreateAsync(user, userDto.Password);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        var token = _tokenGenerator.GenerateToken(user);
        return Ok(new { Token = token });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto userDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userManager.FindByEmailAsync(userDto.Email);
        if (user == null)
            return Unauthorized("Invalid credentials.");

        var result = await _userManager.CheckPasswordAsync(user, userDto.Password);
        if (!result)
            return Unauthorized("Invalid credentials.");

        var token = _tokenGenerator.GenerateToken(user);
        return Ok(new { Token = token });
    }
}