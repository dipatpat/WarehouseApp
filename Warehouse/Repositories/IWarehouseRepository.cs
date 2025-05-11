namespace Warehouse.Repositories;

public interface IWarehouseRepository
{
    public Task<Models.Warehouse?> GetWarehouseByIdAsync(int id, CancellationToken cancellationToken);
}