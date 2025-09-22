using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalInfoShared.Models;

public class Address
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    public Guid PersonId { get; set; }
    
    [Required]
    [MaxLength(20)]
    public string AddressType { get; set; } = string.Empty; // 'Home', 'Work', 'Mailing'
    
    [Required]
    [MaxLength(200)]
    public string StreetAddress { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string City { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(2)]
    public string State { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(10)]
    public string ZipCode { get; set; } = string.Empty;
    
    [MaxLength(2)]
    public string Country { get; set; } = "US";
    
    public bool IsPrimary { get; set; } = false;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public virtual Person Person { get; set; } = null!;
}
