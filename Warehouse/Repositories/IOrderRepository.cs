using Warehouse.Models;

namespace Warehouse.Repositories;

public interface IOrderRepository
{
    public Task<Order?> GetOrderByIdAsync(int idProduct, int amount, DateTime createdAt,
        CancellationToken cancellationToken);

}