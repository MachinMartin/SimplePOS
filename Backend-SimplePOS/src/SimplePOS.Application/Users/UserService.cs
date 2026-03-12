using SimplePOS.Application.Products;
using SimplePOS.Application.Tenants;
using SimplePOS.Domain.Entities;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace SimplePOS.Application.Users;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly ITenantRepository _tenantRepository;

    public UserService(IUserRepository userRepository, ITenantRepository tenantRepository)
    {
        _userRepository = userRepository;
        _tenantRepository = tenantRepository;
    }

    // CREATE USER
    public async Task<UserResponse> CreateAsync(CreateUserRequest request)
    {
        // Check if username is valid
        if (await _userRepository.ExistsByUsernameAsync(request.Username))
        {
            throw new Exception("Username already exists.");
        }

        // Check if tenant exists
        var tenant = await _tenantRepository.GetByIdAsync(request.TenantId);
        if (tenant == null)
            throw new Exception("Tenant not found.");

        // Create user
        var user = new User(request.TenantId, request.Username, request.Password);
        _userRepository.Add(user);
        await _userRepository.SaveChangesAsync();
        return new UserResponse(
            user.Id,
            user.Username,
            new TenantInfo(tenant.Id, tenant.Name)
        );
    }

    // LIST ALL USERS
    public async Task<List<UserResponse>> ListAsync(string? query)
    {
        var users = await _userRepository.ListAsync(query);

        return users
            .Select(x => new UserResponse(
                x.Id,
                x.Username,
                new TenantInfo(
                    x.Tenant.Id,
                    x.Tenant.Name
                )
            ))
            .ToList();
    }

    public async Task<UserResponse> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new Exception("User not found.");

        return new UserResponse(
            user.Id,
            user.Username,
            new TenantInfo(
                user.Tenant.Id,
                user.Tenant.Name
            )
        );
    }

    public async Task UpdateAsync(int id, UpdateUserRequest request)
    {
        var user = await _userRepository.GetTrackedByIdAsync(id);
        if (user == null) throw new Exception("User not found.");
        if (await _userRepository.ExistsByUsernameAsync(request.Username, id))
            throw new Exception("Username already exists.");
        user.UpdateCredentials(request.Username, request.Password);
        await _userRepository.SaveChangesAsync();
    }
}