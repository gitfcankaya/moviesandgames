using Microsoft.AspNetCore.Mvc;
using MoviesAndGames.Core.Entities;
using MoviesAndGames.Core.Interfaces;

namespace MoviesAndGames.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentController : ControllerBase
{
    private readonly IRepository<Comment> _commentRepository;

    public CommentController(IRepository<Comment> commentRepository)
    {
        _commentRepository = commentRepository;
    }

    [HttpGet("content/{contentId}")]
    public async Task<ActionResult<IEnumerable<Comment>>> GetByContentId(int contentId)
    {
        var comments = await _commentRepository.FindAsync(c => c.ContentId == contentId && c.IsApproved);
        return Ok(comments);
    }

    [HttpPost]
    public async Task<ActionResult<Comment>> Create([FromBody] Comment comment)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        comment.IsApproved = false; // Requires approval
        var createdComment = await _commentRepository.AddAsync(comment);
        return CreatedAtAction(nameof(GetByContentId), new { contentId = comment.ContentId }, createdComment);
    }

    [HttpPut("{id}/approve")]
    public async Task<IActionResult> Approve(int id)
    {
        var comment = await _commentRepository.GetByIdAsync(id);
        if (comment == null)
        {
            return NotFound();
        }

        comment.IsApproved = true;
        await _commentRepository.UpdateAsync(comment);
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var comment = await _commentRepository.GetByIdAsync(id);
        if (comment == null)
        {
            return NotFound();
        }

        await _commentRepository.DeleteAsync(comment);
        return NoContent();
    }
}
