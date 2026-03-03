public record CreateTenantRequest(string Name);
public record UpdateTenantRequest(int Id, string Name);
public record TenantResponse(int Id, string Name);