namespace MoviesAndGames.Core.Entities;

public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? ProfileImageUrl { get; set; }
    public string PreferredLanguage { get; set; } = "en";
    public bool IsActive { get; set; } = true;
    public DateTime? LastLoginAt { get; set; }
    
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public virtual ICollection<UserFavorite> Favorites { get; set; } = new List<UserFavorite>();
}
