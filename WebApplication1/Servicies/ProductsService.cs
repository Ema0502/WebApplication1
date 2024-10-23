using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data.Repositories.Interfaces;
using WebApplication1.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Servicies
{
    public class ProductsService : IProductsService
    {
        private readonly IProductRepository _productRepository;
        public ProductsService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            return await _productRepository.GetProducts();
        }

        public async Task<ActionResult<ProductDTO>> GetProduct(Guid id)
        {
            return await _productRepository.GetProduct(id);
        }

        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByName(string name)
        {
            return await _productRepository.GetProductsByName(name);
        }

        public async Task<IActionResult> PutProduct(Guid id, Product product)
        {
            return await _productRepository.PutProduct(id, product);
        }

        public async Task<ActionResult<Product>> PostProduct(ProductDTO productDTO)
        {
            return await _productRepository.PostProduct(productDTO);
        }

        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            return await _productRepository.DeleteProduct(id);
        }
    }
}
