using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data.Repositories.Interfaces;
using WebApplication1.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Servicies
{
    public class ProductsService : IProductsService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public ProductsService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDTO>> GetProducts()
        {
            try
            {
                var listProducts = await _productRepository.GetProducts();
                if (listProducts == null)
                {
                    throw new ArgumentException("No products found in the repository");
                }
                return _mapper.Map<IEnumerable<ProductDTO>>(listProducts);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error", ex);
            }
        }

        public async Task<ActionResult<ProductDTO>> GetProduct(Guid id)
        {
            try
            {
                var product = await _productRepository.GetProduct(id);
                if (product == null)
                {
                    throw new ArgumentException("No product found in the repository");
                }
                return _mapper.Map<ProductDTO>(product);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error", ex);
            }
        }

        public async Task<IEnumerable<ProductDTO>> GetProductsByName(string name)
        {
            try
            {
                var products = await _productRepository.GetProductsByName(name);
                if (products == null)
                {
                    throw new ArgumentException("No product found in the repository");
                }
                return _mapper.Map<IEnumerable<ProductDTO>>(products);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error", ex);
            }
        }

        public async Task<ActionResult<ProductDTO>> PutProduct(Guid id, Product product)
        {
            try
            {
                var updateProduct = await _productRepository.PutProduct(id, product);
                if (updateProduct == null)
                {
                    throw new ArgumentException("No product found in the repository");
                }
                return _mapper.Map<ProductDTO>(updateProduct);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error", ex);
            }
        }

        public async Task<ActionResult<ProductDTO>> PostProduct(Product product)
        {
            try
            {
                var createProduct = await _productRepository.PostProduct(product);
                if (createProduct == null)
                {
                    throw new ArgumentException("Error in the repository");
                }
                return _mapper.Map<ProductDTO>(createProduct);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error", ex);
            }
        }

        public async Task<ActionResult<ProductDTO>> DeleteProduct(Guid id)
        {
            try
            {
                var deleteProduct = await _productRepository.DeleteProduct(id);
                if (deleteProduct == null)
                {
                    throw new ArgumentException("No product found in the repository");
                }
                return _mapper.Map<ProductDTO>(deleteProduct);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error", ex);
            }
        }
    }
}
