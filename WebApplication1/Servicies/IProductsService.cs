using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Servicies
{
    public interface IProductsService
    {
        Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts();
        Task<ActionResult<ProductDTO>> GetProduct(Guid id);
        Task<ActionResult<IEnumerable<Product>>> GetProductsByName(string name);
        Task<IActionResult> PutProduct(Guid id, Product product);
        Task<ActionResult<Product>> PostProduct(ProductDTO productDTO);
        Task<IActionResult> DeleteProduct(Guid id);
    }
}
