using Microsoft.AspNetCore.Mvc;
using SimplePOS.Application.Users;

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

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id) { 
        var user = await _svc.GetByIdAsync(id);
        return Ok(user);
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] string? q)
    {
        var users = await _svc.ListAsync(q);
        return Ok(users);
    }
}