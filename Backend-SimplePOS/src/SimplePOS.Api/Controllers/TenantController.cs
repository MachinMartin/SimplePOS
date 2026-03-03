using Microsoft.AspNetCore.Mvc;
using SimplePOS.Application.Tenants;

namespace SimplePOS.Api.Controllers;

[ApiController]
[Route("api/tenants")]

public class TenantController : ControllerBase
{
    private readonly TenantService _svc;
    public TenantController(TenantService svc)
    {
        _svc = svc;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTenantRequest req)
    {
        var created = await _svc.CreateAsync(req);
        return Created($"/api/tenants/{created.Id}", created);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var tenant = await _svc.GetByIdAsync(id);
        return Ok(tenant);
    }

}