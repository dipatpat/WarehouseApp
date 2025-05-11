using Microsoft.Data.SqlClient;
using Warehouse.Models;

namespace Warehouse.Repositories;

public class OrderRepository : IOrderRepository
{
    
        public readonly string _connectionString;

        public OrderRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<Order?> GetOrderByIdAsync(int idProduct, int amount, DateTime createdAt, CancellationToken cancellationToken)
        {
            await using var con = new SqlConnection(_connectionString);
            await con.OpenAsync(cancellationToken);

            await using var cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"
        SELECT TOP 1 * 
        FROM [Order]
        WHERE IdProduct = @idProduct AND Amount = @amount AND CreatedAt < @createdAt";

            cmd.Parameters.AddWithValue("@idProduct", idProduct);
            cmd.Parameters.AddWithValue("@amount", amount);
            cmd.Parameters.AddWithValue("@createdAt", createdAt);

            using var reader = await cmd.ExecuteReaderAsync(cancellationToken);

            if (await reader.ReadAsync(cancellationToken))
            {
                return new Order
                {
                    IdOrder = (int)reader["IdOrder"],
                    IdProduct = (int)reader["IdProduct"],
                    Amount = (int)reader["Amount"],
                    CreatedAt = (DateTime)reader["CreatedAt"],
                    FulfilledAt = reader["FulfilledAt"] == DBNull.Value ? null : (DateTime?)reader["FulfilledAt"]
                };
            }

            return null;
        }

}