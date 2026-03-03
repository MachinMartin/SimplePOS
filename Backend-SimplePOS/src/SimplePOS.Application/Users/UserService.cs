using SimplePOS.Application.Products;
using SimplePOS.Domain.Entities;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace SimplePOS.Application.Users;

public class UserService
{
    private readonly IUserRepository _repository;
    public UserService(IUserRepository repository) => _repository = repository;

    public async Task<UserResponse> CreateAsync(CreateUserRequest request)
    {
        if (await _repository.ExistsByUsernameAsync(request.Username))
        {
            throw new Exception("Username already exists.");
        }

        var user = new User(request.TenantId, request.Username, request.Password);
        _repository.Add(user);
        await _repository.SaveChangesAsync();
        return new UserResponse(user.Id, user.Username);
    }

    public async Task<List<UserResponse>> ListAsync(string? query)
    {
        var users = await _repository.ListAsync(query);

        return users
            .Select(x => new UserResponse(
                x.Id,
                x.Username
            ))
            .ToList();
    }

    public async Task<UserResponse> GetByIdAsync(int id)
    {
        var user = await _repository.GetByIdAsync(id);
        if (user == null) throw new Exception("User not found.");
        return new UserResponse(user.Id, user.Username);
    }

    public async Task UpdateAsync(int id, UpdateUserRequest request)
    {
        var user = await _repository.GetTrackedByIdAsync(id);
        if (user == null) throw new Exception("User not found.");
        if (await _repository.ExistsByUsernameAsync(request.Username, id))
            throw new Exception("Username already exists.");
        user.UpdateCredentials(request.Username, request.Password);
        await _repository.SaveChangesAsync();
    }
}