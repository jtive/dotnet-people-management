namespace PersonalInfoShared.Services;

public interface IDataMaskingService
{
    string MaskSSN(string? ssn);
    string MaskAddress(string? address);
    string MaskCreditCard(string? cardNumber);
    string MaskBirthDate(DateOnly? birthDate);
    string FormatSSN(string ssn);
    string FormatCreditCard(string cardNumber);
}
