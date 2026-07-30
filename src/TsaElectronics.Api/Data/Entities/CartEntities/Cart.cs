using TsaElectronics.Api.Data.Entities.IdentityEntities;

namespace TsaElectronics.Api.Data.Entities.CartEntities;

public class Cart : BaseEntity
{
    // Set for logged-in users; null for guest carts.
    public Guid? UserId { get; set; }
    public ApplicationUser? User { get; set; }

    // Set for guest carts, matched via a client-side cookie/token; merged into the
    // user's cart on login.
    public string? GuestToken { get; set; }

    public ICollection<CartItem> Items { get; set; } = [];
}
