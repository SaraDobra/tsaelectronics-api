using TsaElectronics.Api.Models.AuthModels;

namespace TsaElectronics.Api.Services.AuthServices;

public interface IAuthService
{
    Task<AuthResponseModel> RegisterAsync(RegisterModel model);
    Task<AuthResponseModel?> LoginAsync(LoginModel model);
}
