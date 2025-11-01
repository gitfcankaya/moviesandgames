using Microsoft.AspNetCore.Mvc;
using MoviesAndGames.Core.Entities;
using MoviesAndGames.Core.Interfaces;

namespace MoviesAndGames.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly IRepository<Category> _categoryRepository;

    public CategoryController(IRepository<Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetAll()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetById(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        
        if (category == null)
        {
            return NotFound();
        }
        
        return Ok(category);
    }

    [HttpGet("slug/{slug}")]
    public async Task<ActionResult<Category>> GetBySlug(string slug)
    {
        var categories = await _categoryRepository.FindAsync(c => c.Slug == slug);
        var category = categories.FirstOrDefault();
        
        if (category == null)
        {
            return NotFound();
        }
        
        return Ok(category);
    }

    [HttpPost]
    public async Task<ActionResult<Category>> Create([FromBody] Category category)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdCategory = await _categoryRepository.AddAsync(category);
        return CreatedAtAction(nameof(GetById), new { id = createdCategory.Id }, createdCategory);
    }
}
