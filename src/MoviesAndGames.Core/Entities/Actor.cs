namespace MoviesAndGames.Core.Entities;

public class Actor : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Biography { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Nationality { get; set; }
    public string? ProfileImageUrl { get; set; }
    
    public virtual ICollection<ContentActor> ContentActors { get; set; } = new List<ContentActor>();
}
