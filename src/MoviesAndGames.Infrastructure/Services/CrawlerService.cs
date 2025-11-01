using HtmlAgilityPack;
using MoviesAndGames.Core.Entities;
using MoviesAndGames.Core.Enums;
using MoviesAndGames.Core.Interfaces;

namespace MoviesAndGames.Infrastructure.Services;

public class CrawlerService : ICrawlerService
{
    private readonly HttpClient _httpClient;

    public CrawlerService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<Content>> CrawlMoviesAsync(string source, int count = 10)
    {
        var contents = new List<Content>();
        
        try
        {
            var html = await _httpClient.GetStringAsync(source);
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            
            // This is a placeholder implementation
            // Actual implementation would depend on the specific website structure
            var movieNodes = htmlDoc.DocumentNode.SelectNodes("//article");
            
            if (movieNodes != null)
            {
                foreach (var node in movieNodes.Take(count))
                {
                    var title = node.SelectSingleNode(".//h2")?.InnerText?.Trim() ?? "Unknown";
                    var description = node.SelectSingleNode(".//p")?.InnerText?.Trim() ?? "";
                    
                    contents.Add(new Content
                    {
                        Title = title,
                        OriginalTitle = title,
                        Description = description,
                        Type = ContentType.Movie,
                        Status = ContentStatus.Draft,
                        Slug = CreateSlug(title),
                        SourceUrl = source,
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }
        }
        catch (Exception ex)
        {
            // Log error
            Console.WriteLine($"Error crawling movies: {ex.Message}");
        }

        return contents;
    }

    public async Task<IEnumerable<Content>> CrawlSeriesAsync(string source, int count = 10)
    {
        var contents = new List<Content>();
        
        try
        {
            var html = await _httpClient.GetStringAsync(source);
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            
            var seriesNodes = htmlDoc.DocumentNode.SelectNodes("//article");
            
            if (seriesNodes != null)
            {
                foreach (var node in seriesNodes.Take(count))
                {
                    var title = node.SelectSingleNode(".//h2")?.InnerText?.Trim() ?? "Unknown";
                    var description = node.SelectSingleNode(".//p")?.InnerText?.Trim() ?? "";
                    
                    contents.Add(new Content
                    {
                        Title = title,
                        OriginalTitle = title,
                        Description = description,
                        Type = ContentType.Series,
                        Status = ContentStatus.Draft,
                        Slug = CreateSlug(title),
                        SourceUrl = source,
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error crawling series: {ex.Message}");
        }

        return contents;
    }

    public async Task<IEnumerable<Content>> CrawlGamesAsync(string source, int count = 10)
    {
        var contents = new List<Content>();
        
        try
        {
            var html = await _httpClient.GetStringAsync(source);
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            
            var gameNodes = htmlDoc.DocumentNode.SelectNodes("//article");
            
            if (gameNodes != null)
            {
                foreach (var node in gameNodes.Take(count))
                {
                    var title = node.SelectSingleNode(".//h2")?.InnerText?.Trim() ?? "Unknown";
                    var description = node.SelectSingleNode(".//p")?.InnerText?.Trim() ?? "";
                    
                    contents.Add(new Content
                    {
                        Title = title,
                        OriginalTitle = title,
                        Description = description,
                        Type = ContentType.Game,
                        Status = ContentStatus.Draft,
                        Slug = CreateSlug(title),
                        SourceUrl = source,
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error crawling games: {ex.Message}");
        }

        return contents;
    }

    private static string CreateSlug(string text)
    {
        return text.ToLowerInvariant()
            .Replace(" ", "-")
            .Replace(".", "")
            .Replace(",", "")
            .Replace("'", "")
            .Replace("\"", "");
    }
}
