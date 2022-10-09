using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Interfaces.Settings;
using ShopApi.Models;
using ShopApi.Repositories;
using ShopApi.Services;

namespace ShopApi.Controllers;

[Route("v1/account")]
public class HomeController : Controller
{
    private readonly ISettingsService _settingsService;

    public HomeController(ISettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    public async Task<ActionResult<dynamic>> Authenticate([FromBody] User model)
    {
        var user = UserRepository.Get(model.Username, model.Password);

        if (user == null)
            return NotFound(new { message = "Usuário ou senha inválidos" });

        var tokenService = new TokenService(_settingsService);
        var token = tokenService.GenerateToken(user);

        user.Password = "";

        return new
        {
            user = user,
            token = token
        };
    }

    [HttpGet]
    [Route("anonymous")]
    [AllowAnonymous]
    public string Anonymous() => "Anônimo";

    [HttpGet]
    [Route("authenticated")]
    [Authorize]
    public string Authenticated() => String.Format("Autenticado - {0}", User.Identity.Name);

    [HttpGet]
    [Route("employee")]
    [Authorize(Roles = "employee,manager")]
    public string Employee() => "Funcionário";

    [HttpGet]
    [Route("manager")]
    [Authorize(Roles = "manager")]
    public string Manager() => "Gerente";
}