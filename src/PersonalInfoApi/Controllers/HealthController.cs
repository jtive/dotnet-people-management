using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalInfoShared.Data;

namespace PersonalInfoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly PersonalInfoDbContext _context;

    public HealthController(PersonalInfoDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<object>> GetHealth()
    {
        try
        {
            // Test database connection
            await _context.Database.CanConnectAsync();
            
            // Get basic statistics
            var personCount = await _context.Persons.CountAsync();
            var addressCount = await _context.Addresses.CountAsync();
            var creditCardCount = await _context.CreditCards.CountAsync();

            return Ok(new
            {
                status = "healthy",
                timestamp = DateTime.UtcNow,
                database = "connected",
                statistics = new
                {
                    persons = personCount,
                    addresses = addressCount,
                    creditCards = creditCardCount
                }
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                status = "unhealthy",
                timestamp = DateTime.UtcNow,
                error = ex.Message
            });
        }
    }

    [HttpGet("ready")]
    public async Task<ActionResult<object>> GetReadiness()
    {
        try
        {
            // Test database connection
            await _context.Database.CanConnectAsync();
            
            return Ok(new
            {
                status = "ready",
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(503, new
            {
                status = "not ready",
                timestamp = DateTime.UtcNow,
                error = ex.Message
            });
        }
    }
}
