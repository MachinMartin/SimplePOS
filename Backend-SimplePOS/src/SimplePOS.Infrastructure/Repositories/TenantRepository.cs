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

    public Task<List<Tenant>> ListAsync(string? q, int? page, int? pageSize)
    {
        var query = _db.Tenants.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
        {
            var term = q.Trim();
            var pattern = $"%{term}%";
            query = query.Where(x =>
                EF.Functions.ILike(x.Name, pattern));
        }
        query = query.OrderBy(x => x.Id);
        // paging
        var p = page.GetValueOrDefault(1);
        var ps = pageSize.GetValueOrDefault(50);
        if (p < 1) p = 1;
        if (ps < 1) ps = 50;
        if (ps > 200) ps = 200;
        query = query.Skip((p - 1) * ps).Take(ps);
        return query.ToListAsync();
    }

    public Task<Tenant?> GetByIdAsync(int id) =>
        _db.Tenants.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

    public Task<Tenant?> GetByNameAsync(string name) =>
        _db.Tenants.AsNoTracking().FirstOrDefaultAsync(x => x.Name == name);

    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}