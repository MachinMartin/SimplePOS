public record CreateUserRequest(int TenantId, string Username, string Password);
public record UpdateUserRequest(int Id, string Username, string Password);
public record UserResponse(int Id, string Username);