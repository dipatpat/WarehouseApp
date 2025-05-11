using Microsoft.Data.SqlClient;
using Warehouse.DTOs;
using Warehouse.Exceptions;
using Warehouse.Models;

namespace Warehouse.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    public readonly string _connectionString;
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;

    public WarehouseRepository(IConfiguration configuration, IProductRepository productRepository, IOrderRepository orderRepository)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
        _productRepository = productRepository;
        _orderRepository = orderRepository;
    }
    public async Task<Models.Warehouse?> GetWarehouseByIdAsync(int id, CancellationToken cancellationToken)
    {
        await using var con = new SqlConnection(_connectionString);
        await con.OpenAsync(cancellationToken);

        await using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "Select * from Warehouse where IdWarehouse = @id";
        cmd.Parameters.AddWithValue("@id", id);

        SqlDataReader reader = await cmd.ExecuteReaderAsync(cancellationToken);

        if (await reader.ReadAsync(cancellationToken))
        {
            return new Models.Warehouse
            {
                IdWarehouse = (int)reader["IdWarehouse"],
                Name = (string)reader["Name"],
                Address = (string)reader["Address"],
            };
        }

        return null;
    }

    public async Task<int?> CreatePurchaseAsync(PurchaseRequestDto request, CancellationToken cancellationToken)
{
    await using var connection = new SqlConnection(_connectionString);
    await connection.OpenAsync(cancellationToken);
    await using var transaction = connection.BeginTransaction();

    try
    {
        if (request.IdProduct <= 0 || request.IdWarehouse <= 0 || request.Amount <= 0)
            throw new BadRequestException("Invalid request data.");

        var checkWarehouseCmd = new SqlCommand("SELECT COUNT(1) FROM Warehouse WHERE IdWarehouse = @id", connection, transaction);
        checkWarehouseCmd.Parameters.AddWithValue("@id", request.IdWarehouse);
        var warehouseExists = (int)await checkWarehouseCmd.ExecuteScalarAsync(cancellationToken);
        if (warehouseExists == 0)
            throw new NotFoundException($"Warehouse with id {request.IdWarehouse} not found.");

        var getProductCmd = new SqlCommand("SELECT Price FROM Product WHERE IdProduct = @id", connection, transaction);
        getProductCmd.Parameters.AddWithValue("@id", request.IdProduct);
        var productPriceObj = await getProductCmd.ExecuteScalarAsync(cancellationToken);
        if (productPriceObj == null)
            throw new NotFoundException($"Product with id {request.IdProduct} not found.");
        var productPrice = (decimal)productPriceObj;

        var getOrderCmd = new SqlCommand(@"
            SELECT TOP 1 IdOrder, CreatedAt, FulfilledAt
            FROM [Order]
            WHERE IdProduct = @idProduct AND Amount = @amount AND CreatedAt < @createdAt", connection, transaction);
        getOrderCmd.Parameters.AddWithValue("@idProduct", request.IdProduct);
        getOrderCmd.Parameters.AddWithValue("@amount", request.Amount);
        getOrderCmd.Parameters.AddWithValue("@createdAt", request.CreatedAt);

        int? orderId = null;
        using (var reader = await getOrderCmd.ExecuteReaderAsync(cancellationToken))
        {
            if (await reader.ReadAsync(cancellationToken))
                orderId = (int)reader["IdOrder"];
            else
                throw new NotFoundException("Matching order not found.");
        }

        var checkDupCmd = new SqlCommand("SELECT COUNT(1) FROM Product_Warehouse WHERE IdOrder = @idOrder", connection, transaction);
        checkDupCmd.Parameters.AddWithValue("@idOrder", orderId);
        var alreadyUsed = (int)await checkDupCmd.ExecuteScalarAsync(cancellationToken);
        if (alreadyUsed > 0)
            throw new ConflictException("Order has already been fulfilled.");

        var updateOrderCmd = new SqlCommand("UPDATE [Order] SET FulfilledAt = @now WHERE IdOrder = @idOrder", connection, transaction);
        updateOrderCmd.Parameters.AddWithValue("@now", DateTime.UtcNow);
        updateOrderCmd.Parameters.AddWithValue("@idOrder", orderId);
        await updateOrderCmd.ExecuteNonQueryAsync(cancellationToken);

        var totalPrice = productPrice * request.Amount;
        var insertCmd = new SqlCommand(@"
            INSERT INTO Product_Warehouse (IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt)
            VALUES (@IdWarehouse, @IdProduct, @IdOrder, @Amount, @Price, @CreatedAt);
            SELECT SCOPE_IDENTITY();", connection, transaction);

        insertCmd.Parameters.AddWithValue("@IdWarehouse", request.IdWarehouse);
        insertCmd.Parameters.AddWithValue("@IdProduct", request.IdProduct);
        insertCmd.Parameters.AddWithValue("@IdOrder", orderId);
        insertCmd.Parameters.AddWithValue("@Amount", request.Amount);
        insertCmd.Parameters.AddWithValue("@Price", totalPrice);
        insertCmd.Parameters.AddWithValue("@CreatedAt", DateTime.UtcNow);

        var insertedId = await insertCmd.ExecuteScalarAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return Convert.ToInt32(insertedId);
    }
    catch
    {
        await transaction.RollbackAsync(cancellationToken);
        throw;
    }
}


}