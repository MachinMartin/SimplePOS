using Microsoft.EntityFrameworkCore;
using SimplePOS.Application.Tenants;
using SimplePOS.Domain.Entities;
using SimplePOS.Infrastructure.Data;

public class TenantRepository : ITenantRepository
{
    private readonly AppDbContext _db;
    public TenantRepository(AppDbContext db) => _db = db;

    public void Add(Tenant tenant) => _db.Tenants.Add(tenant);

    public void Remove(Tenant tenant) => _db.Tenants.Remove(tenant);

    public Task<Tenant?> GetByIdAsync(int id) =>
        _db.Tenants.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

    public Task<Tenant?> GetByNameAsync(string name) =>
        _db.Tenants.AsNoTracking().FirstOrDefaultAsync(x => x.Name == name);

    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}