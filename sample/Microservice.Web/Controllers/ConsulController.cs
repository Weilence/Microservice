using Consul;
using Microservice.Api;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Web.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class ConsulController: ControllerBase
{
    private readonly IUserService _userService;

    public ConsulController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public IActionResult Login(string username, string password)
    {
        var result = _userService.Login(username, password);
        return new JsonResult(result);
    }

    [HttpPost]
    public IActionResult Add()
    {
        _userService.Add(new TestJson());
        return Ok();
    }
}