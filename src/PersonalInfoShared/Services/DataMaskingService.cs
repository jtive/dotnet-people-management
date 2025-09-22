using System.Text.RegularExpressions;

namespace PersonalInfoShared.Services;

public class DataMaskingService : IDataMaskingService
{
    public string MaskSSN(string? ssn)
    {
        if (string.IsNullOrEmpty(ssn))
            return "***-**-****";

        // Remove any non-digits
        var digitsOnly = Regex.Replace(ssn, @"[^\d]", "");
        
        if (digitsOnly.Length != 9)
            return "***-**-****";

        // Show only last 4 digits: ***-**-1234
        var lastFour = digitsOnly.Substring(5, 4);
        return $"***-**-{lastFour}";
    }

    public string MaskAddress(string? address)
    {
        if (string.IsNullOrEmpty(address))
            return "********";

        return "********";
    }

    public string MaskCreditCard(string? cardNumber)
    {
        if (string.IsNullOrEmpty(cardNumber))
            return "****-****-****-****";

        // Remove any non-digits
        var digitsOnly = Regex.Replace(cardNumber, @"[^\d]", "");
        
        if (digitsOnly.Length < 4)
            return "****-****-****-****";

        // Show only last 4 digits
        var lastFour = digitsOnly.Substring(digitsOnly.Length - 4);
        return $"****-****-****-{lastFour}";
    }

    public string MaskBirthDate(DateOnly? birthDate)
    {
        if (!birthDate.HasValue)
            return "********";

        return "********";
    }

    public string FormatSSN(string ssn)
    {
        if (string.IsNullOrEmpty(ssn))
            return string.Empty;

        // Remove any non-digits
        var digitsOnly = Regex.Replace(ssn, @"[^\d]", "");
        
        if (digitsOnly.Length != 9)
            return ssn; // Return original if not 9 digits

        // Format as XXX-XX-XXXX
        return $"{digitsOnly.Substring(0, 3)}-{digitsOnly.Substring(3, 2)}-{digitsOnly.Substring(5, 4)}";
    }

    public string FormatCreditCard(string cardNumber)
    {
        if (string.IsNullOrEmpty(cardNumber))
            return string.Empty;

        // Remove any non-digits
        var digitsOnly = Regex.Replace(cardNumber, @"[^\d]", "");
        
        if (digitsOnly.Length < 13 || digitsOnly.Length > 19)
            return cardNumber; // Return original if invalid length

        // Format based on card type (basic formatting)
        switch (digitsOnly.Length)
        {
            case 13: // Visa (some older cards)
            case 16: // Visa, MasterCard, Discover
                return $"{digitsOnly.Substring(0, 4)}-{digitsOnly.Substring(4, 4)}-{digitsOnly.Substring(8, 4)}-{digitsOnly.Substring(12)}";
            case 15: // American Express
                return $"{digitsOnly.Substring(0, 4)}-{digitsOnly.Substring(4, 6)}-{digitsOnly.Substring(10, 5)}";
            case 14: // Diners Club
                return $"{digitsOnly.Substring(0, 4)}-{digitsOnly.Substring(4, 6)}-{digitsOnly.Substring(10, 4)}";
            default:
                // Generic formatting for other lengths
                var formatted = string.Empty;
                for (int i = 0; i < digitsOnly.Length; i += 4)
                {
                    if (i > 0) formatted += "-";
                    formatted += digitsOnly.Substring(i, Math.Min(4, digitsOnly.Length - i));
                }
                return formatted;
        }
    }
}
