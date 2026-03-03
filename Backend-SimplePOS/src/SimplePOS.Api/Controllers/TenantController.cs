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

    [HttpGet]
    public async Task<IActionResult> List(
        [FromQuery] string? q,
        [FromQuery] int? page,
        [FromQuery] int? pageSize)
    {
        var tenants = await _svc.ListAsync(q, page, pageSize);
        return Ok(tenants);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var tenant = await _svc.GetByIdAsync(id);
        return Ok(tenant);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTenantRequest req)
    {
        var updated = await _svc.UpdateAsync(req);
        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _svc.DeleteAsync(id);
        return NoContent();
    }
}