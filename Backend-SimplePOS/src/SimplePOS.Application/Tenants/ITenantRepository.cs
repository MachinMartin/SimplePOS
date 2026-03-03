using SimplePOS.Domain.Entities;

namespace SimplePOS.Application.Tenants;

public interface ITenantRepository
{
    void Add(Tenant tenant);
    Task<Tenant?> GetByIdAsync(int id);
    Task<Tenant?> GetByNameAsync(string name);
    void Remove(Tenant tenant);
    Task SaveChangesAsync();
}
