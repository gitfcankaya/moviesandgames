using Microsoft.EntityFrameworkCore;
using MoviesAndGames.Core.Entities;

namespace MoviesAndGames.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Content> Contents { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Actor> Actors { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<ContentCategory> ContentCategories { get; set; }
    public DbSet<ContentActor> ContentActors { get; set; }
    public DbSet<ContentTranslation> ContentTranslations { get; set; }
    public DbSet<UserFavorite> UserFavorites { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Content Category (Many-to-Many)
        modelBuilder.Entity<ContentCategory>()
            .HasKey(cc => new { cc.ContentId, cc.CategoryId });

        modelBuilder.Entity<ContentCategory>()
            .HasOne(cc => cc.Content)
            .WithMany(c => c.ContentCategories)
            .HasForeignKey(cc => cc.ContentId);

        modelBuilder.Entity<ContentCategory>()
            .HasOne(cc => cc.Category)
            .WithMany(c => c.ContentCategories)
            .HasForeignKey(cc => cc.CategoryId);

        // Content Actor (Many-to-Many)
        modelBuilder.Entity<ContentActor>()
            .HasKey(ca => new { ca.ContentId, ca.ActorId });

        modelBuilder.Entity<ContentActor>()
            .HasOne(ca => ca.Content)
            .WithMany(c => c.ContentActors)
            .HasForeignKey(ca => ca.ContentId);

        modelBuilder.Entity<ContentActor>()
            .HasOne(ca => ca.Actor)
            .WithMany(a => a.ContentActors)
            .HasForeignKey(ca => ca.ActorId);

        // Category self-referencing relationship
        modelBuilder.Entity<Category>()
            .HasOne(c => c.ParentCategory)
            .WithMany(c => c.SubCategories)
            .HasForeignKey(c => c.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Comment self-referencing relationship
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.ParentComment)
            .WithMany(c => c.Replies)
            .HasForeignKey(c => c.ParentCommentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<Content>()
            .HasIndex(c => c.Slug)
            .IsUnique();

        modelBuilder.Entity<Category>()
            .HasIndex(c => c.Slug)
            .IsUnique();

        modelBuilder.Entity<Actor>()
            .HasIndex(a => a.Slug)
            .IsUnique();

        // Query filters for soft delete
        modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
        modelBuilder.Entity<Content>().HasQueryFilter(c => !c.IsDeleted);
        modelBuilder.Entity<Category>().HasQueryFilter(c => !c.IsDeleted);
        modelBuilder.Entity<Actor>().HasQueryFilter(a => !a.IsDeleted);
        modelBuilder.Entity<Comment>().HasQueryFilter(c => !c.IsDeleted);
    }
}
