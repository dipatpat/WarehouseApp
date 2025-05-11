using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Warehouse.DTOs;
using Warehouse.Services;

namespace Warehouse.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : Controller
{
    private readonly IProductService _productService;  
    private readonly IWarehouseService _warehouseService;

    public ProductController(IProductService productService, IWarehouseService warehouseService)
    {
        _productService = productService;
        _warehouseService = warehouseService;
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductByIdAsync(int id, CancellationToken cancellationToken)
    {
        var item = await _productService.GetProductByIdAsync(id, cancellationToken);
        return Ok(item);
    }
    
    
}