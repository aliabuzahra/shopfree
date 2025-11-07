namespace ShopFree.Domain.ValueObjects;

public record CustomerInfo
{
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;

    private CustomerInfo() { } // For EF Core

    public CustomerInfo(string name, string email, string phone)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Customer name cannot be empty", nameof(name));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Customer email cannot be empty", nameof(email));

        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Customer phone cannot be empty", nameof(phone));

        Name = name;
        Email = email;
        Phone = phone;
    }
}

