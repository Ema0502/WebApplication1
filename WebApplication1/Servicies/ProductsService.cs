using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data.Repositories.Implementations;
using WebApplication1.Data.Repositories.Interfaces;
using WebApplication1.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Servicies
{
    public class ProductsService : IProductsService
    {
        private readonly ProductRepository _productRepository;
        public ProductsService(ProductRepository productRepository) 
        {
            _productRepository = productRepository;
        }
        public Task<ActionResult<ProductDTO>> GetProduct(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult<IEnumerable<Product>>> GetProductsByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult<Product>> PostProduct(ProductDTO productDTO)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> PutProduct(Guid id, Product product)
        {
            throw new NotImplementedException();
        }
        public Task<IActionResult> DeleteProduct(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
