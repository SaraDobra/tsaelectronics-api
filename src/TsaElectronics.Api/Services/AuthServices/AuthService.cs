using Microsoft.AspNetCore.Identity;
using TsaElectronics.Api.Data.Entities.IdentityEntities;
using TsaElectronics.Api.Helpers;
using TsaElectronics.Api.Models.AuthModels;

namespace TsaElectronics.Api.Services.AuthServices;

public class AuthService(
    UserManager<ApplicationUser> userManager,
    JwtTokenHelper jwtTokenHelper) : IAuthService
{
    public async Task<AuthResponseModel> RegisterAsync(RegisterModel model)
    {
        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName
        };

        var result = await userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException(errors);
        }

        await userManager.AddToRoleAsync(user, "Customer");

        return await BuildAuthResponseAsync(user);
    }

    public async Task<AuthResponseModel?> LoginAsync(LoginModel model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);
        if (user is null)
        {
            return null;
        }

        var passwordValid = await userManager.CheckPasswordAsync(user, model.Password);
        if (!passwordValid)
        {
            return null;
        }

        return await BuildAuthResponseAsync(user);
    }

    private async Task<AuthResponseModel> BuildAuthResponseAsync(ApplicationUser user)
    {
        var roles = await userManager.GetRolesAsync(user);
        var token = jwtTokenHelper.GenerateToken(user, roles);

        return new AuthResponseModel
        {
            Token = token,
            UserId = user.Id,
            Email = user.Email ?? string.Empty,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Roles = [.. roles]
        };
    }
}
