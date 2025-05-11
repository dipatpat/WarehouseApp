using Microsoft.Data.SqlClient;
using Warehouse.Models;

namespace Warehouse.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    public readonly string _connectionString;

    public WarehouseRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
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

}