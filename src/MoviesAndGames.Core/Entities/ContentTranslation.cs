namespace MoviesAndGames.Core.Entities;

public class ContentTranslation : BaseEntity
{
    public int ContentId { get; set; }
    public string Language { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public virtual Content Content { get; set; } = null!;
}
