using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Warehouse.Services;

namespace Warehouse.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : Controller
{
    private readonly IProductService _productService;   

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductByIdAsync(int id, CancellationToken cancellationToken)
    {
        var item = await _productService.GetProductByIdAsync(id, cancellationToken);
        return Ok(item);
    }
}