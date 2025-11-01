namespace MoviesAndGames.Core.Entities;

public class ContentActor
{
    public int ContentId { get; set; }
    public int ActorId { get; set; }
    public string? Role { get; set; }
    
    public virtual Content Content { get; set; } = null!;
    public virtual Actor Actor { get; set; } = null!;
}
