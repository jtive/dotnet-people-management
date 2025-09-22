using System.ComponentModel.DataAnnotations;

namespace PersonalInfoShared.DTOs;

public class CreditCardDto
{
    public Guid Id { get; set; }
    public Guid PersonId { get; set; }
    public string CardType { get; set; } = string.Empty;
    public string LastFourDigits { get; set; } = string.Empty; // Masked
    public int ExpirationMonth { get; set; }
    public int ExpirationYear { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateCreditCardDto
{
    [Required(ErrorMessage = "Card Type is required")]
    [StringLength(20, ErrorMessage = "Card Type cannot exceed 20 characters")]
    public string CardType { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Card Number is required")]
    [RegularExpression(@"^\d{13,19}$", ErrorMessage = "Card Number must be 13-19 digits")]
    public string CardNumber { get; set; } = string.Empty; // Full number for creation
    
    [Required(ErrorMessage = "Expiration Month is required")]
    [Range(1, 12, ErrorMessage = "Expiration Month must be between 1 and 12")]
    public int ExpirationMonth { get; set; }
    
    [Required(ErrorMessage = "Expiration Year is required")]
    [Range(2024, 2030, ErrorMessage = "Expiration Year must be between 2024 and 2030")]
    public int ExpirationYear { get; set; }
    
    public bool IsActive { get; set; } = true;
}

public class UpdateCreditCardDto
{
    public string CardType { get; set; } = string.Empty;
    public string CardNumber { get; set; } = string.Empty; // Full number for update
    public int ExpirationMonth { get; set; }
    public int ExpirationYear { get; set; }
    public bool IsActive { get; set; }
}
