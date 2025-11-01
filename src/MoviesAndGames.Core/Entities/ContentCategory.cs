namespace MoviesAndGames.Core.Entities;

public class ContentCategory
{
    public int ContentId { get; set; }
    public int CategoryId { get; set; }
    
    public virtual Content Content { get; set; } = null!;
    public virtual Category Category { get; set; } = null!;
}
