using SimplePOS.Domain.Entities;

namespace SimplePOS.Application.Users;

public interface IUserRepository
{
    void Add(User user);
    Task<bool> ExistsByUsernameAsync(string name, int? excludedId = null);
    Task<User?> GetTrackedByIdAsync(int id);
    Task<User?> GetByIdAsync(int id);
    void Remove(Product product);
    Task SaveChangesAsync();
    Task<List<User>> ListAsync(string? query);
}