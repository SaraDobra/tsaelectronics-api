using System.ComponentModel.DataAnnotations;

namespace TsaElectronics.Api.Models.ProductModels;

public class CreateProductModel
{
    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Brand { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public Guid CategoryId { get; set; }
}
