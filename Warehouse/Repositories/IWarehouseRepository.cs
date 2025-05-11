using Warehouse.DTOs;

namespace Warehouse.Repositories;

public interface IWarehouseRepository
{
    public Task<Models.Warehouse?> GetWarehouseByIdAsync(int id, CancellationToken cancellationToken);
    public Task<int?> CreatePurchaseAsync(PurchaseRequestDto request, CancellationToken cancellationToken);

}