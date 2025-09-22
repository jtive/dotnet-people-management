using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalInfoShared.Data;
using PersonalInfoShared.DTOs;
using PersonalInfoShared.Models;
using PersonalInfoShared.Services;

namespace PersonalInfoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController : ControllerBase
{
    private readonly PersonalInfoDbContext _context;
    private readonly IMappingService _mappingService;

    public PersonController(PersonalInfoDbContext context, IMappingService mappingService)
    {
        _context = context;
        _mappingService = mappingService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PersonDto>>> GetPersons()
    {
        var persons = await _context.Persons
            .Include(p => p.Addresses)
            .Include(p => p.CreditCards)
            .ToListAsync();

        var personDtos = persons.Select(_mappingService.MapToPersonDto).ToList();
        return Ok(personDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PersonDto>> GetPerson(Guid id)
    {
        var person = await _context.Persons
            .Include(p => p.Addresses)
            .Include(p => p.CreditCards)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (person == null)
        {
            return NotFound();
        }

        var personDto = _mappingService.MapToPersonDto(person);
        return Ok(personDto);
    }

    [HttpPost]
    public async Task<ActionResult<PersonDto>> CreatePerson(CreatePersonDto createPersonDto)
    {
        // Validate model state
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var person = _mappingService.MapToPerson(createPersonDto);
            
            _context.Persons.Add(person);
            await _context.SaveChangesAsync();

            var personDto = _mappingService.MapToPersonDto(person);
            return CreatedAtAction(nameof(GetPerson), new { id = person.Id }, personDto);
        }
        catch (Exception ex)
        {
            // Log the exception (in production, use proper logging)
            return StatusCode(500, "An error occurred while creating the person.");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePerson(Guid id, UpdatePersonDto updatePersonDto)
    {
        var person = await _context.Persons.FindAsync(id);
        if (person == null)
        {
            return NotFound();
        }

        _mappingService.UpdatePerson(person, updatePersonDto);
        
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PersonExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePerson(Guid id)
    {
        var person = await _context.Persons.FindAsync(id);
        if (person == null)
        {
            return NotFound();
        }

        _context.Persons.Remove(person);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool PersonExists(Guid id)
    {
        return _context.Persons.Any(e => e.Id == id);
    }
}
