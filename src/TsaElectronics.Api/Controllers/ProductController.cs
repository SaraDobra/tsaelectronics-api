using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TsaElectronics.Api.Models.ProductModels;
using TsaElectronics.Api.Services.ProductServices;

namespace TsaElectronics.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController(IProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ProductModel>>> GetAll([FromQuery] Guid? categoryId, CancellationToken ct)
    {
        return Ok(await productService.GetAllAsync(categoryId, ct));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductModel>> GetById(Guid id, CancellationToken ct)
    {
        var product = await productService.GetByIdAsync(id, ct);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProductModel>> Create(CreateProductModel model, CancellationToken ct)
    {
        var created = await productService.CreateAsync(model, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
}
