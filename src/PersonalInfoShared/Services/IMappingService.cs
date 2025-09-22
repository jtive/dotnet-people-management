using PersonalInfoShared.DTOs;
using PersonalInfoShared.Models;

namespace PersonalInfoShared.Services;

public interface IMappingService
{
    PersonDto MapToPersonDto(Person person);
    Person MapToPerson(CreatePersonDto dto);
    void UpdatePerson(Person person, UpdatePersonDto dto);
    
    AddressDto MapToAddressDto(Address address);
    Address MapToAddress(CreateAddressDto dto);
    void UpdateAddress(Address address, UpdateAddressDto dto);
    
    CreditCardDto MapToCreditCardDto(CreditCard creditCard);
    CreditCard MapToCreditCard(CreateCreditCardDto dto);
    void UpdateCreditCard(CreditCard creditCard, UpdateCreditCardDto dto);
    
}
