using Microsoft.EntityFrameworkCore;
using SimplePOS.Application.Products;
using SimplePOS.Domain.Entities;
using SimplePOS.Infrastructure.Data;

namespace SimplePOS.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _db;
    public ProductRepository(AppDbContext db) => _db = db;

    public void Add(Product product) => _db.Products.Add(product);

    public void Remove(Product product) => _db.Products.Remove(product);

    //The ListAsync method retrieves a list of products from the database, with optional filtering by search term and product group, and supports pagination.
    //It uses AsNoTracking for better performance in read-only scenarios, and it constructs a dynamic query based on the provided parameters.

    public async Task<List<Product>> ListAsync(string? q, int? groupId, int? page, int? pageSize)
    {
        var query = _db.Products.AsNoTracking().AsQueryable();

        if (groupId.HasValue && groupId.Value > 0)
            query = query.Where(x => x.ProductGroupId == groupId.Value);

        if (!string.IsNullOrWhiteSpace(q))
        {
            var term = q.Trim();
            var pattern = $"%{term}%";

            query = query.Where(x =>
                EF.Functions.ILike(x.Name, pattern) ||
                EF.Functions.ILike(x.Sku, pattern));
        }

        query = query.OrderBy(x => x.Id);

        // paging
        var p = page.GetValueOrDefault(1);
        var ps = pageSize.GetValueOrDefault(50);

        if (p < 1) p = 1;
        if (ps < 1) ps = 50;
        if (ps > 200) ps = 200;

        query = query.Skip((p - 1) * ps).Take(ps);

        return await query.ToListAsync();
    }

    //Exclude the current record when checking for duplicates during update operations, to allow the record to be updated without triggering a false positive for a duplicate.
    public Task<bool> ExistsBySkuAsync(string sku, int? excludedId = null)
    {
        var query = _db.Products.AsNoTracking().Where(x => x.Sku == sku);
        if (excludedId.HasValue) query = query.Where(x => x.Id != excludedId.Value);
        return query.AnyAsync();
    }

    public Task<bool> ExistsByNameAsync(string name, int? excludedId = null)
    {
        var query = _db.Products.AsNoTracking().Where(x => x.Name == name);
        if (excludedId.HasValue) query = query.Where(x => x.Id != excludedId.Value);
        return query.AnyAsync();
    }

    //Traked entity is needed for update scenarios, so that EF can detect changes and update the record in the database when SaveChangesAsync is called.
    public Task<Product?> GetTrackedByIdAsync(int id) =>
        _db.Products.FirstOrDefaultAsync(x => x.Id == id);

    //Non-tracked entity is sufficient for read-only scenarios, and it can improve performance by avoiding the overhead of change tracking.
    public Task<Product?> GetByIdAsync(int id) =>
        _db.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

    public Task<Product?> GetBySkuAsync(string sku) =>
        _db.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Sku == sku);
    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}