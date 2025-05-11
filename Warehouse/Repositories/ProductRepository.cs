using Microsoft.Data.SqlClient;
using Warehouse.Models;

namespace Warehouse.Repositories;

public class ProductRepository : IProductRepository
{
    public readonly string _connectionString;

    public ProductRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<Product?> GetProductByIdAsync(int id, CancellationToken cancellationToken)
    {
        await using var con = new SqlConnection(_connectionString);
        await con.OpenAsync(cancellationToken);

        await using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "Select * from Product where IdProduct = @id";
        cmd.Parameters.AddWithValue("@id", id);

        SqlDataReader reader = await cmd.ExecuteReaderAsync(cancellationToken);

        if (await reader.ReadAsync(cancellationToken))
        {
            return new Product
            {
                IdProduct = (int)reader["IdProduct"],
                Name = (string)reader["Name"],
                Description = (string)reader["Description"],
                Price = (decimal)reader["Price"],
            };
        }

        return null;
    }
    


}
    
