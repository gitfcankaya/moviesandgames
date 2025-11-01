namespace MoviesAndGames.Core.Interfaces;

public interface IContentEnhancementService
{
    Task<string> EnhanceContentAsync(string originalContent, string provider = "openai");
    Task<string> TranslateContentAsync(string content, string targetLanguage);
    Task<string> GenerateSummaryAsync(string content);
}
