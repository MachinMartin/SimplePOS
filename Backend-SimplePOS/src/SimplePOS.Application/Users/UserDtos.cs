public record CreateUserRequest(int Id, string Username, string Password, int TenantId);
public record UpdateUserRequest(int Id, string Username, string Password, int TenantId);
public record UserResponse(int Id, string Username, TenantInfo Tenant);
public record TenantInfo(int Id, string Name);