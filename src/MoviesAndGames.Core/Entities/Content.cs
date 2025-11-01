using MoviesAndGames.Core.Enums;

namespace MoviesAndGames.Core.Entities;

public class Content : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string OriginalTitle { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public ContentType Type { get; set; }
    public ContentStatus Status { get; set; }
    public string Language { get; set; } = "en";
    public DateTime? ReleaseDate { get; set; }
    public string? PosterImageUrl { get; set; }
    public string? TrailerUrl { get; set; }
    public double? Rating { get; set; }
    public int? Duration { get; set; }
    public string? Director { get; set; }
    public string? Platform { get; set; }
    public string? SourceUrl { get; set; }
    public bool IsOriginalContent { get; set; } = false;
    public string? TicketmasterEventId { get; set; }
    
    public virtual ICollection<ContentCategory> ContentCategories { get; set; } = new List<ContentCategory>();
    public virtual ICollection<ContentActor> ContentActors { get; set; } = new List<ContentActor>();
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public virtual ICollection<ContentTranslation> Translations { get; set; } = new List<ContentTranslation>();
    public virtual ICollection<UserFavorite> Favorites { get; set; } = new List<UserFavorite>();
}
