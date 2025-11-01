using Microsoft.AspNetCore.Mvc;
using MoviesAndGames.Core.Entities;
using MoviesAndGames.Core.Interfaces;
using MoviesAndGames.Core.Enums;

namespace MoviesAndGames.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContentController : ControllerBase
{
    private readonly IRepository<Content> _contentRepository;
    private readonly ICrawlerService _crawlerService;
    private readonly IContentEnhancementService _enhancementService;

    public ContentController(
        IRepository<Content> contentRepository,
        ICrawlerService crawlerService,
        IContentEnhancementService enhancementService)
    {
        _contentRepository = contentRepository;
        _crawlerService = crawlerService;
        _enhancementService = enhancementService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Content>>> GetAll(
        [FromQuery] ContentType? type = null,
        [FromQuery] string? language = null)
    {
        var contents = await _contentRepository.GetAllAsync();
        
        if (type.HasValue)
        {
            contents = contents.Where(c => c.Type == type.Value);
        }
        
        if (!string.IsNullOrEmpty(language))
        {
            contents = contents.Where(c => c.Language == language);
        }
        
        return Ok(contents);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Content>> GetById(int id)
    {
        var content = await _contentRepository.GetByIdAsync(id);
        
        if (content == null)
        {
            return NotFound();
        }
        
        return Ok(content);
    }

    [HttpGet("slug/{slug}")]
    public async Task<ActionResult<Content>> GetBySlug(string slug)
    {
        var contents = await _contentRepository.FindAsync(c => c.Slug == slug);
        var content = contents.FirstOrDefault();
        
        if (content == null)
        {
            return NotFound();
        }
        
        return Ok(content);
    }

    [HttpPost]
    public async Task<ActionResult<Content>> Create([FromBody] Content content)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdContent = await _contentRepository.AddAsync(content);
        return CreatedAtAction(nameof(GetById), new { id = createdContent.Id }, createdContent);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Content content)
    {
        if (id != content.Id)
        {
            return BadRequest();
        }

        var existingContent = await _contentRepository.GetByIdAsync(id);
        if (existingContent == null)
        {
            return NotFound();
        }

        content.UpdatedAt = DateTime.UtcNow;
        await _contentRepository.UpdateAsync(content);
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var content = await _contentRepository.GetByIdAsync(id);
        if (content == null)
        {
            return NotFound();
        }

        await _contentRepository.DeleteAsync(content);
        return NoContent();
    }

    [HttpPost("crawl")]
    public async Task<ActionResult<IEnumerable<Content>>> CrawlContent(
        [FromQuery] string source,
        [FromQuery] ContentType type,
        [FromQuery] int count = 10)
    {
        IEnumerable<Content> crawledContents;

        switch (type)
        {
            case ContentType.Movie:
                crawledContents = await _crawlerService.CrawlMoviesAsync(source, count);
                break;
            case ContentType.Series:
                crawledContents = await _crawlerService.CrawlSeriesAsync(source, count);
                break;
            case ContentType.Game:
                crawledContents = await _crawlerService.CrawlGamesAsync(source, count);
                break;
            default:
                return BadRequest("Invalid content type");
        }

        return Ok(crawledContents);
    }

    [HttpPost("{id}/enhance")]
    public async Task<ActionResult<string>> EnhanceContent(
        int id,
        [FromQuery] string provider = "openai")
    {
        var content = await _contentRepository.GetByIdAsync(id);
        if (content == null)
        {
            return NotFound();
        }

        var enhancedDescription = await _enhancementService.EnhanceContentAsync(
            content.Description, provider);

        return Ok(new { enhancedDescription });
    }
}
