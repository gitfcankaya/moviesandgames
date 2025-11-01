namespace MoviesAndGames.Core.Interfaces;

public interface ITicketmasterService
{
    Task<string?> GetEventDetailsAsync(string eventId);
    Task<string?> GetTicketPurchaseUrlAsync(string eventId);
    Task<IEnumerable<dynamic>> SearchEventsAsync(string keyword, string? location = null);
}
