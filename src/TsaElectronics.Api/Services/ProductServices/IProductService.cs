using TsaElectronics.Api.Models.ProductModels;

namespace TsaElectronics.Api.Services.ProductServices;

public interface IProductService
{
    Task<List<ProductModel>> GetAllAsync(Guid? categoryId, CancellationToken ct = default);
    Task<ProductModel?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<ProductModel> CreateAsync(CreateProductModel model, CancellationToken ct = default);
}
