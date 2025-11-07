namespace ShopFree.Domain.ValueObjects;

public record Address
{
    public string Street { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string? State { get; init; }
    public string? PostalCode { get; init; }
    public string Country { get; init; } = string.Empty;
    
    private Address() { } // For EF Core
    
    public Address(string street, string city, string country, string? state = null, string? postalCode = null)
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentException("Street cannot be empty", nameof(street));
        
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City cannot be empty", nameof(city));
        
        if (string.IsNullOrWhiteSpace(country))
            throw new ArgumentException("Country cannot be empty", nameof(country));
        
        Street = street;
        City = city;
        Country = country;
        State = state;
        PostalCode = postalCode;
    }
    
    public string GetFullAddress()
    {
        var parts = new List<string> { Street, City };
        if (!string.IsNullOrWhiteSpace(State))
            parts.Add(State);
        if (!string.IsNullOrWhiteSpace(PostalCode))
            parts.Add(PostalCode);
        parts.Add(Country);
        return string.Join(", ", parts);
    }
    
    // Helper method to create from full address string (for simpler API)
    public static Address FromFullAddress(string fullAddress, string country = "Saudi Arabia")
    {
        return new Address(fullAddress, "", country);
    }
}

