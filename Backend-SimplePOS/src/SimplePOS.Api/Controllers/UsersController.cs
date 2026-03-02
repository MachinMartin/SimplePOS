using Microsoft.AspNetCore.Mvc;
using SimplePOS.Application.Products;

namespace SimplePOS.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly UserService _svc;

    public UsersController(UserService svc)
    {
        _svc = svc;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest req)
    {
        var created = await _svc.CreateAsync(req);
        return Created($"/api/users/{created.Id}", created);
    }
}