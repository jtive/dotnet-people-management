namespace PersonalInfoApi.Services;

public interface IRateLimitService
{
    Task<bool> IsAllowedAsync(string clientId, int maxRequests = 1000, TimeSpan? timeWindow = null);
    Task<int> GetRemainingRequestsAsync(string clientId, int maxRequests = 1000, TimeSpan? timeWindow = null);
}
