namespace SimplePOS.Domain.Entities;

public class Tenant
{
    public int Id { get; private set; }
    public string Name { get; private set; } = "";

    public ICollection<User> Users { get; private set; } = new List<User>();

    private Tenant() { } // EF

    public Tenant(string name)
    {
        Name = name;
    }
}
