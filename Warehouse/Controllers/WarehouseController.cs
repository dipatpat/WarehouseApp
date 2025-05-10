using Microsoft.AspNetCore.Mvc;
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
    
    
}