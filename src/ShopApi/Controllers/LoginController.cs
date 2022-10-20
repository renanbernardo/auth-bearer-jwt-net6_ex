using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Interfaces.Settings;
using ShopApi.Models;
using ShopApi.Repositories;
using ShopApi.Services;

namespace ShopApi.Controllers;

[ApiController]
[Route("v1")]
public class LoginController : ControllerBase
{
    private readonly ISettingsService _settingsService;

    public LoginController(ISettingsService settingsService)
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
}