using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using WebApplication1.Dtos;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly HttpClient _httpClient;
        JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        public ProductsController(DataContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            if (products.Count != 0)
            {
                return Ok(products);
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
            return Ok(products);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(Guid id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return ProductToDTO(product);
        }

        // GET: api/Products/search?name=...
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByName([FromQuery] string name)
        {
            var lowerCaseName = name.ToLower().Trim();

            var products = await _context.Products
                                      .Where(p => p.Name.ToLower().Contains(lowerCaseName))
                                      .ToListAsync();
            if (products == null || products.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(products);
            }
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(Guid id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
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
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
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
            };
    }
}
