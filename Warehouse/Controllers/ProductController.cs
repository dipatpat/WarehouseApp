using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Warehouse.Services;

namespace Warehouse.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _productService;   

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        //var item = await _productService.GetByIdAsync(id, cancellationToken);
        return Ok();
    }
}