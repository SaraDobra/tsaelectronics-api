using Microsoft.AspNetCore.Identity;

namespace TsaElectronics.Api.Data.Entities.IdentityEntities;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}
