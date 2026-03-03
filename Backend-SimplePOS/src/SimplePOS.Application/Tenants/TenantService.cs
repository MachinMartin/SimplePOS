using SimplePOS.Application.Tenants;
using SimplePOS.Domain.Entities;


public class TenantService
{
    private readonly ITenantRepository _tenantRepository;
    public TenantService(ITenantRepository tenantRepository)
    {
        _tenantRepository = tenantRepository;
    }

    public async Task<TenantResponse> CreateAsync(CreateTenantRequest request)
    {
        var existingTenant = await _tenantRepository.GetByNameAsync(request.Name);
        if (existingTenant != null)
        {
            throw new Exception("Tenant with the same name already exists.");
        }
        var tenant = new Tenant(request.Name);
        _tenantRepository.Add(tenant);
        await _tenantRepository.SaveChangesAsync();
        return new TenantResponse(tenant.Id, tenant.Name);
    }

    public async Task<TenantResponse> UpdateAsync(UpdateTenantRequest request)
    {
        var tenant = await _tenantRepository.GetByIdAsync(request.Id);
        if (tenant == null)
        {
            throw new Exception("Tenant not found.");
        }
        var existingTenant = await _tenantRepository.GetByNameAsync(request.Name);
        if (existingTenant != null && existingTenant.Id != request.Id)
        {
            throw new Exception("Another tenant with the same name already exists.");
        }
        tenant.ChangeName(request.Name);
        await _tenantRepository.SaveChangesAsync();
        return new TenantResponse(tenant.Id, tenant.Name);
    }

    public async Task<TenantResponse> GetByIdAsync(int id)
    {
        var tenant = await _tenantRepository.GetByIdAsync(id);
        if (tenant == null)
        {
            throw new Exception("Tenant not found.");
        }
        return new TenantResponse(tenant.Id, tenant.Name);
    }

    public async void DeleteAsync(int id)
    {
        var tenant = await _tenantRepository.GetByIdAsync(id);
        if (tenant == null)
        {
            throw new Exception("Tenant not found.");
        }
        _tenantRepository.Remove(tenant);
        await _tenantRepository.SaveChangesAsync();
    }
}