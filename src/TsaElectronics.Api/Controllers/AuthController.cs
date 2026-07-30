using Microsoft.AspNetCore.Mvc;
using TsaElectronics.Api.Models.AuthModels;
using TsaElectronics.Api.Services.AuthServices;

namespace TsaElectronics.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    // Returns the JWT as a JSON body. This endpoint is called server-side by the
    // Next.js app's Auth.js Credentials provider, which wraps the token in its own
    // encrypted session cookie - this API never sets a cookie itself.
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseModel>> Register(RegisterModel model)
    {
        try
        {
            var response = await authService.RegisterAsync(model);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseModel>> Login(LoginModel model)
    {
        var response = await authService.LoginAsync(model);
        if (response is null)
        {
            return Unauthorized(new { error = "Invalid email or password." });
        }

        return Ok(response);
    }
}
