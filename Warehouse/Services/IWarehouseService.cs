namespace Warehouse.Services;
using Warehouse.Models;

public interface IWarehouseService
{
    public Task<Warehouse?> GetWarehouseByIdAsync(int id, CancellationToken cancellationToken);
}