using Warehouse.DTOs;
using Warehouse.Models;

namespace Warehouse.Repositories;

public interface IProductRepository
{
    public Task<Product?> GetProductByIdAsync(int id, CancellationToken cancellationToken);
}