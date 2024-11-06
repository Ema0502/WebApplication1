using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebApplication1.Data.Repositories.Interfaces;
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
        public async Task<IEnumerable<Product>> GetProducts()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://fakestoreapi.com/products/");
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException("Error getting products from external api");
                }

                var content = await response.Content.ReadAsStringAsync();
                var productsApi = JsonSerializer.Deserialize<List<ProductApi>>(content, options);

                var products = productsApi!.Select(product => new Product
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

                return products;
            }
            catch (HttpRequestException httpEx)
            {
                throw new Exception("Error getting products from external api", httpEx);
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Error saving products to database", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error", ex);
            }
        }

        public async Task<ActionResult<Product>> GetProduct(Guid id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);

                if (product == null)
                {
                    return new NotFoundResult();
                }

                return product;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Error getting product in database", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error", ex);
            }
        }

        public async Task<IEnumerable<Product>> GetProductsByName(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new HttpRequestException("Error getting products from external api");
                }

                var lowerCaseName = name.ToLower().Trim();

                var products = await _context.Products
                                          .Where(p => p.Name.ToLower().Contains(lowerCaseName))
                                          .ToListAsync();

                return products;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Error getting products in database", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error", ex);
            }
        }

        public async Task<ActionResult<Product>> PutProduct(Guid id, Product product)
        {
            if (id != product.Id)
            {
                throw new ArgumentException("Product id does not match the provided id");
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return product;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!ProductExists(id))
                {
                    throw new KeyNotFoundException("Product not found");
                }
                else
                {
                    throw new InvalidOperationException("Concurrency error while updating the product", ex);
                }
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Error updating the database. Please try again later", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error", ex);
            }
        }


        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            try
            {
                var user = await _context.Users.FindAsync(product.UserId);

                if (user == null)
                {
                    return new NotFoundResult();
                }

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                return product;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Error creating a product in database", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error", ex);
            }
        }

        public async Task<ActionResult<Product>> DeleteProduct(Guid id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return new NotFoundResult();
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return product;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Error getting product in database", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error", ex);
            }
        }

        private bool ProductExists(Guid id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
