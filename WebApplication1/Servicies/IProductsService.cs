using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Servicies
{
    public interface IProductsService
    {
        Task<IEnumerable<ProductDTO>> GetProducts();
        Task<ActionResult<ProductDTO>> GetProduct(Guid id);
        Task<IEnumerable<ProductDTO>> GetProductsByName(string name);
        Task<ActionResult<ProductDTO>> PutProduct(Guid id, Product product);
        Task<ActionResult<ProductDTO>> PostProduct(Product product);
        Task<ActionResult<ProductDTO>> DeleteProduct(Guid id);
    }
}
