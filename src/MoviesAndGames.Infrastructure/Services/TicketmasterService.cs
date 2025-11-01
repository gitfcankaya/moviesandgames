using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MoviesAndGames.Core.Interfaces;
using System.Text.Json;

namespace MoviesAndGames.Infrastructure.Services;

public class TicketmasterService : ITicketmasterService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TicketmasterService> _logger;
    private readonly string _apiKey;
    private const string BaseUrl = "https://app.ticketmaster.com/discovery/v2";

    public TicketmasterService(HttpClient httpClient, IConfiguration configuration, ILogger<TicketmasterService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _apiKey = configuration["Ticketmaster:ApiKey"] ?? string.Empty;
    }

    public async Task<string?> GetEventDetailsAsync(string eventId)
    {
        try
        {
            var url = $"{BaseUrl}/events/{eventId}?apikey={_apiKey}";
            var response = await _httpClient.GetStringAsync(url);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting event details for event ID: {EventId}", eventId);
            return null;
        }
    }

    public async Task<string?> GetTicketPurchaseUrlAsync(string eventId)
    {
        try
        {
            var eventDetails = await GetEventDetailsAsync(eventId);
            if (eventDetails != null)
            {
                using var doc = JsonDocument.Parse(eventDetails);
                var url = doc.RootElement.GetProperty("url").GetString();
                return url;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting ticket purchase URL for event ID: {EventId}", eventId);
        }
        
        return null;
    }

    public async Task<IEnumerable<dynamic>> SearchEventsAsync(string keyword, string? location = null)
    {
        var events = new List<dynamic>();
        
        try
        {
            var url = $"{BaseUrl}/events?apikey={_apiKey}&keyword={keyword}";
            if (!string.IsNullOrEmpty(location))
            {
                url += $"&city={location}";
            }
            
            var response = await _httpClient.GetStringAsync(url);
            using var doc = JsonDocument.Parse(response);
            
            if (doc.RootElement.TryGetProperty("_embedded", out var embedded))
            {
                if (embedded.TryGetProperty("events", out var eventsArray))
                {
                    foreach (var eventItem in eventsArray.EnumerateArray())
                    {
                        events.Add(eventItem);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching events with keyword: {Keyword}, location: {Location}", keyword, location ?? "none");
        }
        
        return events;
    }
}
