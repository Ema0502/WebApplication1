using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Data.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<ActionResult<Product>> GetProduct(Guid id);
        Task<IEnumerable<Product>> GetProductsByName(string name);
        Task<ActionResult<Product>> PutProduct(Guid id, Product product);
        Task<ActionResult<Product>> PostProduct(Product product);
        Task<ActionResult<Product>> DeleteProduct(Guid id);
    }
}
