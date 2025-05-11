using Warehouse.DTOs;

namespace Warehouse.Services;
using Warehouse.Models;

public interface IWarehouseService
{
    public Task<int?> CreatePurchaseAsync(PurchaseRequestDto request, CancellationToken cancellationToken);
    public Task<int?> CreatePurchaseByProcAsync(PurchaseRequestDto request, CancellationToken cancellationToken);
}