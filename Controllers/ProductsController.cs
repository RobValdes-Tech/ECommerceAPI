using ECommerceApi.Data;
using ECommerceApi.Models.DTOs;
using ECommerceApi.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace ECommerceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/products
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                Stock = dto.Stock
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        // GET: api/products/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound(new { Message = "Producto no encontrado." });
            return Ok(product);
        }

        // GET: api/products/catalog?pageNumber=1&pageSize=10
        [HttpGet("catalog")]
        public async Task<IActionResult> GetCatalog([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1 || pageSize > 50) pageSize = 10;

            var totalRecords = await _context.Products
                .AsNoTracking()
                .Where(p => p.Stock > 0)
                .CountAsync();

            var products = await _context.Products
                .AsNoTracking()
                .Where(p => p.Stock > 0)
                .OrderBy(p => p.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var response = new PagedResponseDto<Product>(products, pageNumber, pageSize, totalRecords);
            return Ok(response);
        }
    }
}
