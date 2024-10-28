using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pets.DTOs.Requests.Auth;
using Pets.Interfaces;
using Pets.Models;
using Pets.Utils;

namespace Pets.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUsersService _service;

    public UsersController(IUsersService service)
    {
        _service = service;
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult> Login([FromBody] LoginDTO request)
    {
        var token = await _service.Login(request);

        return Ok(token);
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        return Ok(await _service.GetUsers());
    }

    [HttpPost]
    public async Task<ActionResult<User>> Post(User user)
    {
        try
        {
            await _service.AddUser(user);

            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<ActionResult<User>> Put([FromRoute] int id, [FromBody] User user)
    {
        try
        {
            await _service.UpdateUser(id, user);

            return Ok(user);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<ActionResult<User>> Delete([FromRoute] int id)
    {
        try
        {
            await _service.DeleteUser(id);

            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}