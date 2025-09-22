using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalInfoShared.Data;
using PersonalInfoShared.DTOs;
using PersonalInfoShared.Models;
using PersonalInfoShared.Services;

namespace PersonalInfoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CreditCardController : ControllerBase
{
    private readonly PersonalInfoDbContext _context;
    private readonly IMappingService _mappingService;

    public CreditCardController(PersonalInfoDbContext context, IMappingService mappingService)
    {
        _context = context;
        _mappingService = mappingService;
    }

    [HttpGet("person/{personId}")]
    public async Task<ActionResult<IEnumerable<CreditCardDto>>> GetCreditCardsByPerson(Guid personId)
    {
        var creditCards = await _context.CreditCards
            .Where(c => c.PersonId == personId)
            .ToListAsync();

        var creditCardDtos = creditCards.Select(_mappingService.MapToCreditCardDto).ToList();
        return Ok(creditCardDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CreditCardDto>> GetCreditCard(Guid id)
    {
        var creditCard = await _context.CreditCards.FindAsync(id);
        if (creditCard == null)
        {
            return NotFound();
        }

        var creditCardDto = _mappingService.MapToCreditCardDto(creditCard);
        return Ok(creditCardDto);
    }

    [HttpPost("person/{personId}")]
    public async Task<ActionResult<CreditCardDto>> CreateCreditCard(Guid personId, CreateCreditCardDto createCreditCardDto)
    {
        // Verify person exists
        var person = await _context.Persons.FindAsync(personId);
        if (person == null)
        {
            return NotFound("Person not found");
        }

        var creditCard = _mappingService.MapToCreditCard(createCreditCardDto);
        creditCard.PersonId = personId;
        
        _context.CreditCards.Add(creditCard);
        await _context.SaveChangesAsync();

        var creditCardDto = _mappingService.MapToCreditCardDto(creditCard);
        return CreatedAtAction(nameof(GetCreditCard), new { id = creditCard.Id }, creditCardDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCreditCard(Guid id, UpdateCreditCardDto updateCreditCardDto)
    {
        var creditCard = await _context.CreditCards.FindAsync(id);
        if (creditCard == null)
        {
            return NotFound();
        }

        _mappingService.UpdateCreditCard(creditCard, updateCreditCardDto);
        
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CreditCardExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCreditCard(Guid id)
    {
        var creditCard = await _context.CreditCards.FindAsync(id);
        if (creditCard == null)
        {
            return NotFound();
        }

        _context.CreditCards.Remove(creditCard);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CreditCardExists(Guid id)
    {
        return _context.CreditCards.Any(e => e.Id == id);
    }
}
