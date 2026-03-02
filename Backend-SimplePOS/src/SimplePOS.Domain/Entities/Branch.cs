namespace SimplePOS.Domain.Entities;

public class Branch
{
    public int Id { get; private set; }
    public string Name { get; private set; } = "";

    public int TenantId { get; private set; }
    public Tenant Tenant { get; private set; } = null!;

    private Branch() { } // EF

    public Branch(int tenantId, string name)
    {
        TenantId = tenantId;
        Name = name;
    }
}
