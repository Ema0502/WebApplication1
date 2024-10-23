using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json;
using WebApplication1.Data.Repositories.Interfaces;
using WebApplication1.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Data.Repositories.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;
        private readonly HttpClient _httpClient;
        JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        public ProductRepository(DataContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            if (products.Count != 0)
            {
                return new OkObjectResult(products);
            }
            else
            {
                var response = await _httpClient.GetAsync("https://fakestoreapi.com/products/");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var productsApi = JsonSerializer.Deserialize<List<ProductApi>>(content, options);

                    products = productsApi!.Select(product => new Product
                    {
                        Id = Guid.NewGuid(),
                        Name = product.Title,
                        Feature = product.Description,
                        PublicationDate = DateTime.Now,
                        Image = product.Image,
                        Price = (int)product.Price,
                        ConditionProd = "new",
                        UserId = new Guid("3fa85f64-5717-4562-b3fc-2c963f55afa6")
                    }).ToList();

                    await _context.Products.AddRangeAsync(products);
                    await _context.SaveChangesAsync();
                }
            }
            return new OkObjectResult(products);
        }

        public async Task<ActionResult<ProductDTO>> GetProduct(Guid id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return new NotFoundResult();
            }

            return ProductToDTO(product);
        }

        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByName(string name)
        {
            var lowerCaseName = name.ToLower().Trim();

            var products = await _context.Products
                                      .Where(p => p.Name.ToLower().Contains(lowerCaseName))
                                      .ToListAsync();
            if (products == null || products.Count == 0)
            {
                return new NotFoundResult();
            }
            else
            {
                return new OkObjectResult(products);
            }
        }

        public async Task<IActionResult> PutProduct(Guid id, Product product)
        {
            if (id != product.Id)
            {
                return new BadRequestResult();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return new NotFoundResult();
                }
                else
                {
                    throw;
                }
            }

            return new NoContentResult();
        }

        public async Task<ActionResult<Product>> PostProduct(ProductDTO productDTO)
        {
            var user = await _context.Users.FindAsync(productDTO.UserId);

            if (user == null)
            {
                return new NotFoundResult();
            }

            var product = new Product
            {
                Name = productDTO.Name,
                Feature = productDTO.Feature,
                PublicationDate = productDTO.PublicationDate,
                Image = productDTO.Image,
                Price = productDTO.Price,
                ConditionProd = productDTO.ConditionProd,
                UserId = productDTO.UserId,
                User = user
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return new NotFoundResult();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }

        private bool ProductExists(Guid id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        private static ProductDTO ProductToDTO(Product product) =>
            new ProductDTO
            {
                Name = product.Name,
                Feature = product.Feature,
                PublicationDate = product.PublicationDate,
                Image = product.Image,
                Price = product.Price,
                ConditionProd = product.ConditionProd,
                UserId = product.UserId,
            };
    }
}
