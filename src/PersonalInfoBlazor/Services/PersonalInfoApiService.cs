using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using PersonalInfoShared.DTOs;

namespace PersonalInfoBlazor.Services;

public class PersonalInfoApiService : IPersonalInfoApiService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public PersonalInfoApiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        // Check environment variable first, then configuration, then default
        var baseUrl = Environment.GetEnvironmentVariable("API_BASE_URL") 
                     ?? configuration["ApiSettings:BaseUrl"] 
                     ?? "https://6pz6yc2hhw.us-east-2.awsapprunner.com";
        _httpClient.BaseAddress = new Uri(baseUrl);
        
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            Converters = { 
                new JsonStringEnumConverter(),
                new FlexibleDateTimeConverter()
            }
        };
    }

    // Person operations
    public async Task<IEnumerable<PersonDto>> GetPersonsAsync()
    {
        var response = await _httpClient.GetAsync("/api/person");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<PersonDto>>(json, _jsonOptions) ?? new List<PersonDto>();
    }

    public async Task<PersonDto?> GetPersonAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"/api/person/{id}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<PersonDto>(json, _jsonOptions);
    }

    public async Task<PersonDto> CreatePersonAsync(CreatePersonDto person)
    {
        var json = JsonSerializer.Serialize(person, _jsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("/api/person", content);
        response.EnsureSuccessStatusCode();
        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<PersonDto>(responseJson, _jsonOptions)!;
    }

    public async Task UpdatePersonAsync(Guid id, UpdatePersonDto person)
    {
        var json = JsonSerializer.Serialize(person, _jsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"/api/person/{id}", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeletePersonAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"/api/person/{id}");
        response.EnsureSuccessStatusCode();
    }

    // Address operations
    public async Task<IEnumerable<AddressDto>> GetAddressesByPersonAsync(Guid personId)
    {
        var response = await _httpClient.GetAsync($"/api/address/person/{personId}");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<AddressDto>>(json, _jsonOptions) ?? new List<AddressDto>();
    }

    public async Task<AddressDto?> GetAddressAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"/api/address/{id}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<AddressDto>(json, _jsonOptions);
    }

    public async Task<UnmaskedAddressDto?> GetUnmaskedAddressAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"/api/address/{id}/unmasked");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<UnmaskedAddressDto>(json, _jsonOptions);
    }

    public async Task<AddressDto> CreateAddressAsync(Guid personId, CreateAddressDto address)
    {
        var json = JsonSerializer.Serialize(address, _jsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"/api/address/person/{personId}", content);
        response.EnsureSuccessStatusCode();
        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<AddressDto>(responseJson, _jsonOptions)!;
    }

    public async Task UpdateAddressAsync(Guid id, UpdateAddressDto address)
    {
        var json = JsonSerializer.Serialize(address, _jsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"/api/address/{id}", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAddressAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"/api/address/{id}");
        response.EnsureSuccessStatusCode();
    }

    // Credit Card operations
    public async Task<IEnumerable<CreditCardDto>> GetCreditCardsByPersonAsync(Guid personId)
    {
        var response = await _httpClient.GetAsync($"/api/creditcard/person/{personId}");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<CreditCardDto>>(json, _jsonOptions) ?? new List<CreditCardDto>();
    }

    public async Task<CreditCardDto?> GetCreditCardAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"/api/creditcard/{id}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CreditCardDto>(json, _jsonOptions);
    }

    public async Task<CreditCardDto> CreateCreditCardAsync(Guid personId, CreateCreditCardDto creditCard)
    {
        var json = JsonSerializer.Serialize(creditCard, _jsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"/api/creditcard/person/{personId}", content);
        response.EnsureSuccessStatusCode();
        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CreditCardDto>(responseJson, _jsonOptions)!;
    }

    public async Task UpdateCreditCardAsync(Guid id, UpdateCreditCardDto creditCard)
    {
        var json = JsonSerializer.Serialize(creditCard, _jsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"/api/creditcard/{id}", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteCreditCardAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"/api/creditcard/{id}");
        response.EnsureSuccessStatusCode();
    }

    // Health check
    public async Task<HealthDto> GetHealthAsync()
    {
        var response = await _httpClient.GetAsync("/api/health");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<HealthDto>(json, _jsonOptions)!;
    }
}

// Custom DateTime converter to handle various date formats
public class FlexibleDateTimeConverter : JsonConverter<DateTime>
{
    private static readonly string[] DateFormats = {
        "yyyy-MM-ddTHH:mm:ss.fffZ",
        "yyyy-MM-ddTHH:mm:ssZ",
        "yyyy-MM-ddTHH:mm:ss.fff",
        "yyyy-MM-ddTHH:mm:ss",
        "yyyy-MM-dd HH:mm:ss",
        "yyyy-MM-dd",
        "MM/dd/yyyy",
        "dd/MM/yyyy",
        "yyyy-MM-ddTHH:mm:ss.fffK", // With timezone offset
        "yyyy-MM-ddTHH:mm:ssK",     // With timezone offset
        "yyyy-MM-ddTHH:mm:ss.fffzzz", // With timezone
        "yyyy-MM-ddTHH:mm:sszzz"    // With timezone
    };

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var dateString = reader.GetString();
            
            if (string.IsNullOrEmpty(dateString))
                return DateTime.MinValue;

            // Handle masked data - if the date is masked, return a default date
            if (dateString == "********" || dateString.StartsWith("***"))
            {
                return DateTime.MinValue; // Return default date for masked data
            }

            // Try parsing with different formats
            foreach (var format in DateFormats)
            {
                if (DateTime.TryParseExact(dateString, format, null, System.Globalization.DateTimeStyles.None, out var result))
                {
                    return result;
                }
            }

            // Fallback to standard DateTime parsing
            if (DateTime.TryParse(dateString, out var fallbackResult))
            {
                return fallbackResult;
            }

            throw new JsonException($"Unable to parse date: {dateString}");
        }

        if (reader.TokenType == JsonTokenType.Number)
        {
            // Handle Unix timestamp
            var timestamp = reader.GetInt64();
            return DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime;
        }

        throw new JsonException($"Unexpected token type: {reader.TokenType}");
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));
    }
}
