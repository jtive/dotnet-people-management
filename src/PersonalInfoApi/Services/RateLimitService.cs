using System.Collections.Concurrent;

namespace PersonalInfoApi.Services;

public class RateLimitService : IRateLimitService
{
    private readonly ConcurrentDictionary<string, List<DateTime>> _requestHistory = new();
    private readonly TimeSpan _defaultTimeWindow = TimeSpan.FromDays(1);

    public Task<bool> IsAllowedAsync(string clientId, int maxRequests = 1000, TimeSpan? timeWindow = null)
    {
        var window = timeWindow ?? _defaultTimeWindow;
        var now = DateTime.UtcNow;
        var cutoff = now - window;

        // Get or create request history for this client
        var requests = _requestHistory.GetOrAdd(clientId, _ => new List<DateTime>());

        // Remove old requests outside the time window
        requests.RemoveAll(r => r < cutoff);

        // Check if we're under the limit
        if (requests.Count >= maxRequests)
        {
            return Task.FromResult(false);
        }

        // Add current request
        requests.Add(now);
        return Task.FromResult(true);
    }

    public Task<int> GetRemainingRequestsAsync(string clientId, int maxRequests = 1000, TimeSpan? timeWindow = null)
    {
        var window = timeWindow ?? _defaultTimeWindow;
        var now = DateTime.UtcNow;
        var cutoff = now - window;

        // Get request history for this client
        if (!_requestHistory.TryGetValue(clientId, out var requests))
        {
            return Task.FromResult(maxRequests);
        }

        // Remove old requests outside the time window
        requests.RemoveAll(r => r < cutoff);

        // Calculate remaining requests
        var remaining = maxRequests - requests.Count;
        return Task.FromResult(Math.Max(0, remaining));
    }
}
