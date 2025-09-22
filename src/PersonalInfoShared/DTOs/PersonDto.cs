using System.ComponentModel.DataAnnotations;

namespace PersonalInfoShared.DTOs;

public class PersonDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string SSN { get; set; } = string.Empty; // Masked
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<AddressDto> Addresses { get; set; } = new();
    public List<CreditCardDto> CreditCards { get; set; } = new();
}

public class CreatePersonDto
{
    [Required(ErrorMessage = "First Name is required")]
    [StringLength(50, ErrorMessage = "First Name cannot exceed 50 characters")]
    public string FirstName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Last Name is required")]
    [StringLength(50, ErrorMessage = "Last Name cannot exceed 50 characters")]
    public string LastName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Birth Date is required")]
    public DateTime BirthDate { get; set; }
    
    [Required(ErrorMessage = "SSN is required")]
    [RegularExpression(@"^\d{9}$", ErrorMessage = "SSN must be exactly 9 digits")]
    public string SSN { get; set; } = string.Empty;
    
    public List<CreateAddressDto> Addresses { get; set; } = new();
    public List<CreateCreditCardDto> CreditCards { get; set; } = new();
}

public class UpdatePersonDto
{
    [Required(ErrorMessage = "First Name is required")]
    [StringLength(50, ErrorMessage = "First Name cannot exceed 50 characters")]
    public string FirstName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Last Name is required")]
    [StringLength(50, ErrorMessage = "Last Name cannot exceed 50 characters")]
    public string LastName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Birth Date is required")]
    public DateTime BirthDate { get; set; }
}
