using PersonalInfoApi.Services;
using System.Net;

namespace PersonalInfoApi.Middleware;

public class RateLimitMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IRateLimitService _rateLimitService;
    private readonly ILogger<RateLimitMiddleware> _logger;

    public RateLimitMiddleware(RequestDelegate next, IRateLimitService rateLimitService, ILogger<RateLimitMiddleware> logger)
    {
        _next = next;
        _rateLimitService = rateLimitService;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Only apply rate limiting to write operations (POST, PUT, DELETE)
        if (IsWriteOperation(context.Request.Method))
        {
            var clientId = GetClientId(context);
            var isAllowed = await _rateLimitService.IsAllowedAsync(clientId, maxRequests: 1000, TimeSpan.FromDays(1));

            if (!isAllowed)
            {
                var remaining = await _rateLimitService.GetRemainingRequestsAsync(clientId, maxRequests: 1000, TimeSpan.FromDays(1));
                
                _logger.LogWarning("Rate limit exceeded for client {ClientId}. Remaining requests: {Remaining}", clientId, remaining);
                
                context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                context.Response.ContentType = "application/json";
                
                var response = new
                {
                    error = "Rate limit exceeded",
                    message = "You have exceeded the maximum number of write operations (1000 per day). Please try again tomorrow.",
                    remainingRequests = remaining,
                    resetTime = DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-ddTHH:mm:ssZ")
                };
                
                await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
                return;
            }
        }

        await _next(context);
    }

    private static bool IsWriteOperation(string method)
    {
        return method.Equals("POST", StringComparison.OrdinalIgnoreCase) ||
               method.Equals("PUT", StringComparison.OrdinalIgnoreCase) ||
               method.Equals("DELETE", StringComparison.OrdinalIgnoreCase);
    }

    private static string GetClientId(HttpContext context)
    {
        // Try to get client IP address
        var clientIp = context.Connection.RemoteIpAddress?.ToString();
        
        // If behind a proxy, try to get the real IP
        if (context.Request.Headers.ContainsKey("X-Forwarded-For"))
        {
            clientIp = context.Request.Headers["X-Forwarded-For"].FirstOrDefault()?.Split(',').FirstOrDefault()?.Trim();
        }
        else if (context.Request.Headers.ContainsKey("X-Real-IP"))
        {
            clientIp = context.Request.Headers["X-Real-IP"].FirstOrDefault();
        }

        return clientIp ?? "unknown";
    }
}
