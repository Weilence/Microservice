using Consul;
using Microservice.Service;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Web.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class ConsulController : ControllerBase
{
    private readonly IConsulClient _consulClient;
    private readonly IUserService _userService;

    public ConsulController(IConsulClient consulClient, IUserService userService)
    {
        _consulClient = consulClient;
        _userService = userService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        var result = _userService.Login("admin", "123456");
        return Ok(result);
    }
}