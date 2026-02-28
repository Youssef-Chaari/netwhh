using BiProject.Api.Data.Oltp;
using BiProject.Api.Data.Oltp.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BiProject.Api.Controllers
{
    [ApiController]
    [Route("api/categories")]
    [Authorize(Roles = "Admin, User")]
    public class CategoryController : ControllerBase
    {
        private readonly OltpDbContext _context;

        public CategoryController(OltpDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _context.ProductCategories
                .Select(c => new { c.Id, c.Name })
                .ToListAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _context.ProductCategories.FindAsync(id);
            if (category == null) return NotFound();
            return Ok(new { category.Id, category.Name });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CategoryDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest(new { message = "Name is required." });

            var category = new ProductCategory { Name = dto.Name.Trim() };
            _context.ProductCategories.Add(category);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, new { category.Id, category.Name });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryDto dto)
        {
            var category = await _context.ProductCategories.FindAsync(id);
            if (category == null) return NotFound();

            category.Name = dto.Name.Trim();
            await _context.SaveChangesAsync();
            return Ok(new { category.Id, category.Name });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.ProductCategories.FindAsync(id);
            if (category == null) return NotFound();

            var hasProducts = await _context.Products.AnyAsync(p => p.CategoryId == id);
            if (hasProducts)
                return BadRequest(new { message = "Cannot delete category with associated products." });

            _context.ProductCategories.Remove(category);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    public class CategoryDto
    {
        public string Name { get; set; } = string.Empty;
    }
}
