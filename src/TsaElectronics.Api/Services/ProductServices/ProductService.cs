using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TsaElectronics.Api.Data;
using TsaElectronics.Api.Data.Entities.ProductEntities;
using TsaElectronics.Api.Helpers;
using TsaElectronics.Api.Models.ProductModels;

namespace TsaElectronics.Api.Services.ProductServices;

public class ProductService(AppDbContext db, IMapper mapper) : IProductService
{
    public async Task<List<ProductModel>> GetAllAsync(Guid? categoryId, CancellationToken ct = default)
    {
        var query = db.Products.AsNoTracking();

        if (categoryId is not null)
        {
            query = query.Where(p => p.CategoryId == categoryId);
        }

        return await query
            .ProjectTo<ProductModel>(mapper.ConfigurationProvider)
            .ToListAsync(ct);
    }

    public async Task<ProductModel?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await db.Products
            .AsNoTracking()
            .Where(p => p.Id == id)
            .ProjectTo<ProductModel>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<ProductModel> CreateAsync(CreateProductModel model, CancellationToken ct = default)
    {
        var product = mapper.Map<Product>(model);
        product.Slug = SlugHelper.Generate(product.Name);

        db.Products.Add(product);
        await db.SaveChangesAsync(ct);

        return mapper.Map<ProductModel>(product);
    }
}
