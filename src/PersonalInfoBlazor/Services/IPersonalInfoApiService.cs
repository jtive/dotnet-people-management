using PersonalInfoShared.DTOs;

namespace PersonalInfoBlazor.Services;

public interface IPersonalInfoApiService
{
    // Person operations
    Task<IEnumerable<PersonDto>> GetPersonsAsync();
    Task<PersonDto?> GetPersonAsync(Guid id);
    Task<PersonDto> CreatePersonAsync(CreatePersonDto person);
    Task UpdatePersonAsync(Guid id, UpdatePersonDto person);
    Task DeletePersonAsync(Guid id);
    // Address operations
    Task<IEnumerable<AddressDto>> GetAddressesByPersonAsync(Guid personId);
    Task<AddressDto?> GetAddressAsync(Guid id);
    Task<AddressDto> CreateAddressAsync(Guid personId, CreateAddressDto address);
    Task UpdateAddressAsync(Guid id, UpdateAddressDto address);
    Task DeleteAddressAsync(Guid id);

    // Credit Card operations
    Task<IEnumerable<CreditCardDto>> GetCreditCardsByPersonAsync(Guid personId);
    Task<CreditCardDto?> GetCreditCardAsync(Guid id);
    Task<CreditCardDto> CreateCreditCardAsync(Guid personId, CreateCreditCardDto creditCard);
    Task UpdateCreditCardAsync(Guid id, UpdateCreditCardDto creditCard);
    Task DeleteCreditCardAsync(Guid id);

    // Health check
    Task<HealthDto> GetHealthAsync();
}
