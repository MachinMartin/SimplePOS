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

    public Task<List<User>> ListAsync() =>
        _db.Users.AsNoTracking().OrderBy(x => x.Id).ToListAsync();

    public Task<bool> ExistsByUsernameAsync(string username, int? excludedId = null)
    {
        var query = _db.Users.AsNoTracking().Where(x => x.Username == username);
        if (excludedId.HasValue) query = query.Where(x => x.Id != excludedId.Value);
        return query.AnyAsync();
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

    Task<IEnumerable<object>> IUserRepository.ListAsync()
    {
        throw new NotImplementedException();
    }
}