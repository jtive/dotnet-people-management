using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalInfoShared.Data;
using PersonalInfoShared.DTOs;
using PersonalInfoShared.Models;
using PersonalInfoShared.Services;

namespace PersonalInfoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AddressController : ControllerBase
{
    private readonly PersonalInfoDbContext _context;
    private readonly IMappingService _mappingService;

    public AddressController(PersonalInfoDbContext context, IMappingService mappingService)
    {
        _context = context;
        _mappingService = mappingService;
    }

    [HttpGet("person/{personId}")]
    public async Task<ActionResult<IEnumerable<AddressDto>>> GetAddressesByPerson(Guid personId)
    {
        var addresses = await _context.Addresses
            .Where(a => a.PersonId == personId)
            .ToListAsync();

        var addressDtos = addresses.Select(_mappingService.MapToAddressDto).ToList();
        return Ok(addressDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AddressDto>> GetAddress(Guid id)
    {
        var address = await _context.Addresses.FindAsync(id);
        if (address == null)
        {
            return NotFound();
        }

        var addressDto = _mappingService.MapToAddressDto(address);
        return Ok(addressDto);
    }

    [HttpGet("{id}/unmasked")]
    public async Task<ActionResult<UnmaskedAddressDto>> GetUnmaskedAddress(Guid id)
    {
        var address = await _context.Addresses.FindAsync(id);
        if (address == null)
        {
            return NotFound();
        }

        var addressDto = _mappingService.MapToUnmaskedAddressDto(address);
        return Ok(addressDto);
    }

    [HttpPost("person/{personId}")]
    public async Task<ActionResult<AddressDto>> CreateAddress(Guid personId, CreateAddressDto createAddressDto)
    {
        // Verify person exists
        var person = await _context.Persons.FindAsync(personId);
        if (person == null)
        {
            return NotFound("Person not found");
        }

        var address = _mappingService.MapToAddress(createAddressDto);
        address.PersonId = personId;
        
        _context.Addresses.Add(address);
        await _context.SaveChangesAsync();

        var addressDto = _mappingService.MapToAddressDto(address);
        return CreatedAtAction(nameof(GetAddress), new { id = address.Id }, addressDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAddress(Guid id, UpdateAddressDto updateAddressDto)
    {
        var address = await _context.Addresses.FindAsync(id);
        if (address == null)
        {
            return NotFound();
        }

        _mappingService.UpdateAddress(address, updateAddressDto);
        
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AddressExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAddress(Guid id)
    {
        var address = await _context.Addresses.FindAsync(id);
        if (address == null)
        {
            return NotFound();
        }

        _context.Addresses.Remove(address);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool AddressExists(Guid id)
    {
        return _context.Addresses.Any(e => e.Id == id);
    }
}
