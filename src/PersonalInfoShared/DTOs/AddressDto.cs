using System.ComponentModel.DataAnnotations;

namespace PersonalInfoShared.DTOs;

public class AddressDto
{
    public Guid Id { get; set; }
    public Guid PersonId { get; set; }
    public string AddressType { get; set; } = string.Empty;
    public string StreetAddress { get; set; } = string.Empty; // Masked
    public string City { get; set; } = string.Empty; // Masked
    public string State { get; set; } = string.Empty; // Masked
    public string ZipCode { get; set; } = string.Empty; // Masked
    public string Country { get; set; } = string.Empty; // Masked
    public bool IsPrimary { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateAddressDto
{
    [Required(ErrorMessage = "Address Type is required")]
    [StringLength(20, ErrorMessage = "Address Type cannot exceed 20 characters")]
    public string AddressType { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Street Address is required")]
    [StringLength(200, ErrorMessage = "Street Address cannot exceed 200 characters")]
    public string StreetAddress { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "City is required")]
    [StringLength(100, ErrorMessage = "City cannot exceed 100 characters")]
    public string City { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "State is required")]
    [StringLength(2, MinimumLength = 2, ErrorMessage = "State must be exactly 2 characters")]
    public string State { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Zip Code is required")]
    [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Zip Code must be in format 12345 or 12345-6789")]
    public string ZipCode { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Country is required")]
    [StringLength(2, MinimumLength = 2, ErrorMessage = "Country must be exactly 2 characters")]
    public string Country { get; set; } = "US";
    
    public bool IsPrimary { get; set; }
}

public class UpdateAddressDto
{
    public string AddressType { get; set; } = string.Empty;
    public string StreetAddress { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }
}
