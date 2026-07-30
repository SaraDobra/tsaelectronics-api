using AutoMapper;
using TsaElectronics.Api.Data.Entities.ProductEntities;
using TsaElectronics.Api.Models.ProductModels;

namespace TsaElectronics.Api.Mapper;

public class ProductMapper : Profile
{
    public ProductMapper()
    {
        CreateMap<Product, ProductModel>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.Images.Select(i => i.Url)));

        CreateMap<ProductVariant, ProductVariantModel>();

        CreateMap<CreateProductModel, Product>();
    }
}
