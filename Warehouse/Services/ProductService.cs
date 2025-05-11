using Warehouse.Exceptions;
using Warehouse.Models;
using Warehouse.Repositories;

namespace Warehouse.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    public async Task<Product?> GetProductByIdAsync(int id, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetProductByIdAsync(id, cancellationToken);
        if (product == null)
        {
            throw new NotFoundException($"Product with id {id} could not be found.");
        }
        return product;
    }
}