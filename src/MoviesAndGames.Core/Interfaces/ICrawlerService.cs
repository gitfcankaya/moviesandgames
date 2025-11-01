using MoviesAndGames.Core.Entities;

namespace MoviesAndGames.Core.Interfaces;

public interface ICrawlerService
{
    Task<IEnumerable<Content>> CrawlMoviesAsync(string source, int count = 10);
    Task<IEnumerable<Content>> CrawlSeriesAsync(string source, int count = 10);
    Task<IEnumerable<Content>> CrawlGamesAsync(string source, int count = 10);
}
