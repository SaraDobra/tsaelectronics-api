using TsaElectronics.Api.Data.Entities.IdentityEntities;

namespace TsaElectronics.Api.Data.Entities.ProductEntities;

public class Review : BaseEntity
{
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;

    public int Rating { get; set; } // 1-5
    public string? Comment { get; set; }
}
