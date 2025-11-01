namespace MoviesAndGames.Core.Entities;

public class UserFavorite : BaseEntity
{
    public int UserId { get; set; }
    public int ContentId { get; set; }
    
    public virtual User User { get; set; } = null!;
    public virtual Content Content { get; set; } = null!;
}
