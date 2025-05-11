using Microsoft.AspNetCore.Mvc;
using Warehouse.DTOs;
using Warehouse.Services;

namespace Warehouse.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WarehouseController : ControllerBase
{
    private readonly IWarehouseService _warehouseService;   

    public WarehouseController(IWarehouseService warehouseService)
    {
            _warehouseService = warehouseService;
    }


    [HttpPost]
    public async Task<IActionResult> CreatePurchaseAsync(PurchaseRequestDto request, CancellationToken cancellationToken)
    {
        var transactionId = await _warehouseService.CreatePurchaseAsync(request, cancellationToken);
        return StatusCode(201, new { IdProductWarehouse = transactionId });
    }
    
    [HttpPost("proc")]
    public async Task<IActionResult> CreatePurchaseByProcAsync(PurchaseRequestDto request, CancellationToken cancellationToken)
    {
        var transactionId = await _warehouseService.CreatePurchaseByProcAsync(request, cancellationToken);
        return StatusCode(201, new { IdProductWarehouse = transactionId });
    }
    
}