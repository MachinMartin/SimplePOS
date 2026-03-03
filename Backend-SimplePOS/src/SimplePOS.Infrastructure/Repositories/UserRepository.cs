using Microsoft.EntityFrameworkCore;
using SimplePOS.Application.Users;
using SimplePOS.Domain.Entities;
using SimplePOS.Infrastructure.Data;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;
    public UserRepository(AppDbContext db) => _db = db;

    public void Add(User user) => _db.Users.Add(user);

    public void Remove(User user) => _db.Users.Remove(user);

public Task<bool> ExistsByUsernameAsync(string username, int? excludedId = null)
    {
        var query = _db.Users.AsNoTracking().Where(x => x.Username == username);
        if (excludedId.HasValue) query = query.Where(x => x.Id != excludedId.Value);
        return query.AnyAsync();
    }

    public async Task<List<User>> ListAsync(string? query)
    {
        var users = _db.Users.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(query))
            users = users.Where(x => x.Username.Contains(query));

        return await users
            .OrderBy(x => x.Id)
            .ToListAsync();
    }

    public Task<User?> GetTrackedByIdAsync(int id) =>
        _db.Users.FirstOrDefaultAsync(x => x.Id == id);

    public Task SaveChangesAsync() => _db.SaveChangesAsync();

    public Task<User?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public void Remove(Product product)
    {
        throw new NotImplementedException();
    }

    
}