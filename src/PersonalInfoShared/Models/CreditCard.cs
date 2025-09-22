using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalInfoShared.Models;

public class CreditCard
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    public Guid PersonId { get; set; }
    
    [Required]
    [MaxLength(20)]
    public string CardType { get; set; } = string.Empty; // 'Visa', 'MasterCard', 'Amex', etc.
    
    [Required]
    [MaxLength(4)]
    public string LastFourDigits { get; set; } = string.Empty;
    
    [Required]
    [Range(1, 12)]
    public int ExpirationMonth { get; set; }
    
    [Required]
    [Range(2024, 2030)]
    public int ExpirationYear { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public virtual Person Person { get; set; } = null!;
}
