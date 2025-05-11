using Warehouse.DTOs;

namespace Warehouse.Services;
using Warehouse.Models;

public interface IWarehouseService
{
    public Task<Warehouse?> GetWarehouseByIdAsync(int id, CancellationToken cancellationToken);
    public Task<int?> CreatePurchaseAsync(PurchaseRequestDto request, CancellationToken cancellationToken);
}