using Microsoft.AspNetCore.Mvc;
using PersonalInfoApi.Services;

namespace PersonalInfoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RateLimitController : ControllerBase
{
    private readonly IRateLimitService _rateLimitService;
    private readonly ILogger<RateLimitController> _logger;

    public RateLimitController(IRateLimitService rateLimitService, ILogger<RateLimitController> logger)
    {
        _rateLimitService = rateLimitService;
        _logger = logger;
    }

    [HttpGet("status")]
    public async Task<IActionResult> GetRateLimitStatus()
    {
        var clientId = GetClientId();
        var remaining = await _rateLimitService.GetRemainingRequestsAsync(clientId, maxRequests: 1000, TimeSpan.FromDays(1));
        
        return Ok(new
        {
            clientId = clientId,
            remainingRequests = remaining,
            maxRequests = 1000,
            timeWindow = "24 hours",
            resetTime = DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-ddTHH:mm:ssZ")
        });
    }

    private string GetClientId()
    {
        // Try to get client IP address
        var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString();
        
        // If behind a proxy, try to get the real IP
        if (HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
        {
            clientIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()?.Split(',').FirstOrDefault()?.Trim();
        }
        else if (HttpContext.Request.Headers.ContainsKey("X-Real-IP"))
        {
            clientIp = HttpContext.Request.Headers["X-Real-IP"].FirstOrDefault();
        }

        return clientIp ?? "unknown";
    }
}
