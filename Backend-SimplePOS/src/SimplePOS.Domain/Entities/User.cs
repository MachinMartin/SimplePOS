
namespace SimplePOS.Domain.Entities;

public class User
{
    public int Id { get; private set; }

    public string Username { get; private set; } = "";
    public string Password { get; private set; } = "";

    public bool HasAccessToPOS { get; private set; } = false;
    public bool HasAccessToBO { get; private set; } = false;
    public bool IsAdmin { get; private set; } = false;

    public int TenantId { get; private set; }
    public Tenant Tenant { get; private set; } = null!;


    private User() { } // EF

    public User(int tenantId, string username, string password)
    {
        TenantId = tenantId;
        Username = username;
        Password = password;
    }

    public void UpdateCredentials(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username cannot be empty");

        Username = username;
        Password = password;
    }
}
