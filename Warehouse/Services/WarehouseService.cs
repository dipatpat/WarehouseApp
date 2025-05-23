using Microsoft.AspNetCore.WebUtilities;
using Warehouse.DTOs;
using Warehouse.Exceptions;
using Warehouse.Repositories;

namespace Warehouse.Services;
using Warehouse.Models;

public class WarehouseService : IWarehouseService
{
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IOrderRepository _orderRepository;

    public WarehouseService(IWarehouseRepository warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;
    }

    public async Task<Warehouse?> GetWarehouseByIdAsync(int id, CancellationToken cancellationToken)
    {
        var warehouse = await _warehouseRepository.GetWarehouseByIdAsync(id, cancellationToken);
        if (warehouse == null)
        {
            throw new NotFoundException("Warehouse with id: " + id + " could not be found.");
        }

        return warehouse;
    }

    public async Task<int?> CreatePurchaseAsync(PurchaseRequestDto request, CancellationToken cancellationToken)
    {
        var id = await _warehouseRepository.CreatePurchaseAsync(request, cancellationToken);
        return id;
    }

    public async Task<int?> CreatePurchaseByProcAsync(PurchaseRequestDto request, CancellationToken cancellationToken)
    {
        var id = await _warehouseRepository.CreatePurchaseByProcAsync(request, cancellationToken);
        return id;

    }

}