namespace MoviesAndGames.Core.Entities;

public class Comment : BaseEntity
{
    public int UserId { get; set; }
    public int ContentId { get; set; }
    public int? ParentCommentId { get; set; }
    public string Text { get; set; } = string.Empty;
    public bool IsApproved { get; set; } = false;
    
    public virtual User User { get; set; } = null!;
    public virtual Content Content { get; set; } = null!;
    public virtual Comment? ParentComment { get; set; }
    public virtual ICollection<Comment> Replies { get; set; } = new List<Comment>();
}
