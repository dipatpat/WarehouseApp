using Warehouse.Repositories;

namespace Warehouse.Services;
using Warehouse.Models;

public class WarehouseService : IWarehouseService
{
    private readonly IWarehouseRepository _warehouseRepository;

    public WarehouseService(IWarehouseRepository productRepository)
    {
        _warehouseRepository = productRepository;
    }
    public async Task<Warehouse?> GetWarehouseByIdAsync(int id, CancellationToken cancellationToken)
    {
        var warehouse = await _warehouseRepository.GetWarehouseByIdAsync(id, cancellationToken);
        return warehouse;
    }

}