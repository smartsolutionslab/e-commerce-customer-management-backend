using System.Text.RegularExpressions;

namespace E_Commerce.CustomerManagement.Domain.ValueObjects;

public record PhoneNumber
{
    private static readonly Regex PhoneRegex = new(@"^\+?[1-9]\d{1,14}$", RegexOptions.Compiled);

    public string Value { get; }
    public string CountryCode { get; }
    public string Number { get; }

    private PhoneNumber(string value, string countryCode, string number)
    {
        Value = value;
        CountryCode = countryCode;
        Number = number;
    }

    public static PhoneNumber Create(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Phone number cannot be empty", nameof(phoneNumber));

        var cleanNumber = phoneNumber.Trim().Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");
        
        if (!PhoneRegex.IsMatch(cleanNumber))
            throw new ArgumentException("Invalid phone number format", nameof(phoneNumber));

        var countryCode = "";
        var number = cleanNumber;
        
        if (cleanNumber.StartsWith("+"))
        {
            if (cleanNumber.Length > 3)
            {
                countryCode = cleanNumber[..3];
                number = cleanNumber[3..];
            }
        }

        return new PhoneNumber(cleanNumber, countryCode, number);
    }

    public override string ToString() => Value;

    public static implicit operator string(PhoneNumber phoneNumber) => phoneNumber.Value;
}
