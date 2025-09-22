using PersonalInfoShared.DTOs;
using PersonalInfoShared.Models;
using System.Text.RegularExpressions;

namespace PersonalInfoShared.Services;

public class MappingService : IMappingService
{
    private readonly IDataMaskingService _maskingService;

    public MappingService(IDataMaskingService maskingService)
    {
        _maskingService = maskingService;
    }

    public PersonDto MapToPersonDto(Person person)
    {
        return new PersonDto
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            BirthDate = person.BirthDate,
            SSN = _maskingService.MaskSSN(person.SSN),
            CreatedAt = person.CreatedAt,
            UpdatedAt = person.UpdatedAt,
            Addresses = person.Addresses.Select(MapToAddressDto).ToList(),
            CreditCards = person.CreditCards.Select(MapToCreditCardDto).ToList()
        };
    }

    public Person MapToPerson(CreatePersonDto dto)
    {
        return new Person
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            BirthDate = DateTime.SpecifyKind(dto.BirthDate, DateTimeKind.Utc),
            SSN = _maskingService.FormatSSN(dto.SSN), // Format for database compatibility
            Addresses = dto.Addresses.Select(MapToAddress).ToList(),
            CreditCards = dto.CreditCards.Select(MapToCreditCard).ToList()
        };
    }

    public void UpdatePerson(Person person, UpdatePersonDto dto)
    {
        person.FirstName = dto.FirstName;
        person.LastName = dto.LastName;
        person.BirthDate = DateTime.SpecifyKind(dto.BirthDate, DateTimeKind.Utc);
        // SSN is not editable, so we don't update it
        person.UpdatedAt = DateTime.UtcNow;
    }

    public AddressDto MapToAddressDto(Address address)
    {
        return new AddressDto
        {
            Id = address.Id,
            PersonId = address.PersonId,
            AddressType = address.AddressType,
            StreetAddress = _maskingService.MaskAddress(address.StreetAddress),
            City = _maskingService.MaskAddress(address.City),
            State = _maskingService.MaskAddress(address.State),
            ZipCode = _maskingService.MaskAddress(address.ZipCode),
            Country = _maskingService.MaskAddress(address.Country),
            IsPrimary = address.IsPrimary,
            CreatedAt = address.CreatedAt,
            UpdatedAt = address.UpdatedAt
        };
    }

    public UnmaskedAddressDto MapToUnmaskedAddressDto(Address address)
    {
        return new UnmaskedAddressDto
        {
            Id = address.Id,
            PersonId = address.PersonId,
            AddressType = address.AddressType,
            StreetAddress = address.StreetAddress,
            City = address.City,
            State = address.State,
            ZipCode = address.ZipCode,
            Country = address.Country,
            IsPrimary = address.IsPrimary,
            CreatedAt = address.CreatedAt,
            UpdatedAt = address.UpdatedAt
        };
    }

    public Address MapToAddress(CreateAddressDto dto)
    {
        return new Address
        {
            AddressType = dto.AddressType,
            StreetAddress = dto.StreetAddress,
            City = dto.City,
            State = dto.State,
            ZipCode = dto.ZipCode,
            Country = dto.Country,
            IsPrimary = dto.IsPrimary
        };
    }

    public void UpdateAddress(Address address, UpdateAddressDto dto)
    {
        address.AddressType = dto.AddressType;
        address.StreetAddress = dto.StreetAddress;
        address.City = dto.City;
        address.State = dto.State;
        address.ZipCode = dto.ZipCode;
        address.Country = dto.Country;
        address.IsPrimary = dto.IsPrimary;
        address.UpdatedAt = DateTime.UtcNow;
    }

    public CreditCardDto MapToCreditCardDto(CreditCard creditCard)
    {
        return new CreditCardDto
        {
            Id = creditCard.Id,
            PersonId = creditCard.PersonId,
            CardType = creditCard.CardType,
            LastFourDigits = $"****-****-****-{creditCard.LastFourDigits}",
            ExpirationMonth = creditCard.ExpirationMonth,
            ExpirationYear = creditCard.ExpirationYear,
            IsActive = creditCard.IsActive,
            CreatedAt = creditCard.CreatedAt,
            UpdatedAt = creditCard.UpdatedAt
        };
    }

    public CreditCard MapToCreditCard(CreateCreditCardDto dto)
    {
        var digitsOnly = Regex.Replace(dto.CardNumber, @"[^\d]", "");
        var lastFour = digitsOnly.Length >= 4 ? digitsOnly.Substring(digitsOnly.Length - 4) : "";

        return new CreditCard
        {
            CardType = dto.CardType,
            LastFourDigits = lastFour,
            ExpirationMonth = dto.ExpirationMonth,
            ExpirationYear = dto.ExpirationYear,
            IsActive = dto.IsActive
        };
    }

    public void UpdateCreditCard(CreditCard creditCard, UpdateCreditCardDto dto)
    {
        var digitsOnly = Regex.Replace(dto.CardNumber, @"[^\d]", "");
        var lastFour = digitsOnly.Length >= 4 ? digitsOnly.Substring(digitsOnly.Length - 4) : "";

        creditCard.CardType = dto.CardType;
        creditCard.LastFourDigits = lastFour;
        creditCard.ExpirationMonth = dto.ExpirationMonth;
        creditCard.ExpirationYear = dto.ExpirationYear;
        creditCard.IsActive = dto.IsActive;
        creditCard.UpdatedAt = DateTime.UtcNow;
    }

}
