using Microsoft.EntityFrameworkCore;
using MoviesAndGames.Core.Entities;
using MoviesAndGames.Core.Enums;

namespace MoviesAndGames.Infrastructure.Data;

public static class SeedData
{
    public static async Task Initialize(ApplicationDbContext context)
    {
        await context.Database.MigrateAsync();

        if (await context.Categories.AnyAsync())
        {
            return; // DB has been seeded
        }

        // Seed Categories
        var categories = new[]
        {
            new Category { Name = "Action", Slug = "action", Description = "Action movies, series and games" },
            new Category { Name = "Adventure", Slug = "adventure", Description = "Adventure content" },
            new Category { Name = "Comedy", Slug = "comedy", Description = "Comedy content" },
            new Category { Name = "Drama", Slug = "drama", Description = "Drama content" },
            new Category { Name = "Horror", Slug = "horror", Description = "Horror content" },
            new Category { Name = "Sci-Fi", Slug = "sci-fi", Description = "Science Fiction content" },
            new Category { Name = "Thriller", Slug = "thriller", Description = "Thriller content" },
            new Category { Name = "Romance", Slug = "romance", Description = "Romance content" },
            new Category { Name = "Fantasy", Slug = "fantasy", Description = "Fantasy content" },
            new Category { Name = "Animation", Slug = "animation", Description = "Animated content" },
            new Category { Name = "RPG", Slug = "rpg", Description = "Role-playing games" },
            new Category { Name = "FPS", Slug = "fps", Description = "First-person shooter games" },
            new Category { Name = "Strategy", Slug = "strategy", Description = "Strategy games" },
            new Category { Name = "Sports", Slug = "sports", Description = "Sports games and content" }
        };

        await context.Categories.AddRangeAsync(categories);
        await context.SaveChangesAsync();

        // Seed Actors
        var actors = new[]
        {
            new Actor 
            { 
                Name = "Tom Hanks", 
                Slug = "tom-hanks", 
                Biography = "American actor and filmmaker",
                Nationality = "American",
                BirthDate = new DateTime(1956, 7, 9)
            },
            new Actor 
            { 
                Name = "Meryl Streep", 
                Slug = "meryl-streep", 
                Biography = "American actress",
                Nationality = "American",
                BirthDate = new DateTime(1949, 6, 22)
            },
            new Actor 
            { 
                Name = "Leonardo DiCaprio", 
                Slug = "leonardo-dicaprio", 
                Biography = "American actor and producer",
                Nationality = "American",
                BirthDate = new DateTime(1974, 11, 11)
            },
            new Actor 
            { 
                Name = "Jennifer Lawrence", 
                Slug = "jennifer-lawrence", 
                Biography = "American actress",
                Nationality = "American",
                BirthDate = new DateTime(1990, 8, 15)
            },
            new Actor 
            { 
                Name = "Denzel Washington", 
                Slug = "denzel-washington", 
                Biography = "American actor and director",
                Nationality = "American",
                BirthDate = new DateTime(1954, 12, 28)
            }
        };

        await context.Actors.AddRangeAsync(actors);
        await context.SaveChangesAsync();

        // Seed Sample Content
        var contents = new[]
        {
            new Content
            {
                Title = "The Matrix",
                OriginalTitle = "The Matrix",
                Description = "A computer hacker learns from mysterious rebels about the true nature of his reality.",
                Slug = "the-matrix",
                Type = ContentType.Movie,
                Status = ContentStatus.Published,
                Language = "en",
                ReleaseDate = new DateTime(1999, 3, 31),
                Rating = 8.7,
                Duration = 136,
                Director = "The Wachowskis",
                Platform = "Warner Bros"
            },
            new Content
            {
                Title = "Breaking Bad",
                OriginalTitle = "Breaking Bad",
                Description = "A high school chemistry teacher turned methamphetamine producer.",
                Slug = "breaking-bad",
                Type = ContentType.Series,
                Status = ContentStatus.Published,
                Language = "en",
                ReleaseDate = new DateTime(2008, 1, 20),
                Rating = 9.5,
                Platform = "AMC"
            },
            new Content
            {
                Title = "The Last of Us",
                OriginalTitle = "The Last of Us",
                Description = "An action-adventure game in a post-apocalyptic world.",
                Slug = "the-last-of-us",
                Type = ContentType.Game,
                Status = ContentStatus.Published,
                Language = "en",
                ReleaseDate = new DateTime(2013, 6, 14),
                Rating = 9.3,
                Platform = "PlayStation"
            },
            new Content
            {
                Title = "Inception",
                OriginalTitle = "Inception",
                Description = "A thief who steals corporate secrets through dream-sharing technology.",
                Slug = "inception",
                Type = ContentType.Movie,
                Status = ContentStatus.Published,
                Language = "en",
                ReleaseDate = new DateTime(2010, 7, 16),
                Rating = 8.8,
                Duration = 148,
                Director = "Christopher Nolan",
                Platform = "Warner Bros"
            },
            new Content
            {
                Title = "Stranger Things",
                OriginalTitle = "Stranger Things",
                Description = "When a young boy disappears, his mother and friends must confront supernatural forces.",
                Slug = "stranger-things",
                Type = ContentType.Series,
                Status = ContentStatus.Published,
                Language = "en",
                ReleaseDate = new DateTime(2016, 7, 15),
                Rating = 8.7,
                Platform = "Netflix"
            },
            new Content
            {
                Title = "Red Dead Redemption 2",
                OriginalTitle = "Red Dead Redemption 2",
                Description = "An action-adventure game set in the Wild West.",
                Slug = "red-dead-redemption-2",
                Type = ContentType.Game,
                Status = ContentStatus.Published,
                Language = "en",
                ReleaseDate = new DateTime(2018, 10, 26),
                Rating = 9.7,
                Platform = "Rockstar Games"
            }
        };

        await context.Contents.AddRangeAsync(contents);
        await context.SaveChangesAsync();

        // Seed sample user
        var user = new User
        {
            Username = "admin",
            Email = "admin@moviesandgames.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
            FirstName = "Admin",
            LastName = "User",
            PreferredLanguage = "en",
            IsActive = true
        };

        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }
}
