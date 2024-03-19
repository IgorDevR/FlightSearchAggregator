using FlightSearchAggregator.Example;
using FlightSearchAggregator.Helpers;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace FlightSearchAggregator.Auth;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly ILogger _logger;

    public AuthController(AuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterModel model)
    {
        _logger.LogInformationWithMethod($"RegisterModel: {model}");
        var result = _authService.Register(model.FullName, model.Email, model.Password);
        if (!result)
        {
            return BadRequest("User already exists.");
        }

        return Ok("User registered successfully.");
    }

    [HttpPost("login")]
    [SwaggerOperation(Summary = "",
        Description = "Use the login in the example or register")]
    [SwaggerRequestExample(typeof(string), typeof(LoginExample))]
    public IActionResult Login([FromBody] LoginModel model)
    {
        _logger.LogInformationWithMethod($"LoginModel: {model}");
        var token = _authService.Login(model.Email, model.Password);
        if (token == null)
        {
            return Unauthorized("Invalid email or password.");
        }

        return Ok(new { Token = token });
    }
}