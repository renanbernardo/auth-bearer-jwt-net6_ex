using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
        var refreshToken = TokenService.GenerateRefreshToken();
        TokenService.SaveRefreshToken(user.Username, refreshToken);

        user.Password = "";

        return new
        {
            user = user,
            token = token,
            refreshToken = refreshToken
        };
    }

    [HttpPost]
    [Route("refresh")]
    public IActionResult Refresh(string token, string refreshToken)
    {
        var tokenService = new TokenService(_settingsService);
        var principal = tokenService.GetPrincipalFromExpiredToken(token);
        var username = principal.Identity.Name;
        var savedRefreshToken = TokenService.GetRefreshToken(username);
        if (savedRefreshToken != refreshToken)
            throw new SecurityTokenException("Invalid refresh token");

        var newJwt = tokenService.GenerateToken(principal.Claims);
        var newRefreshToken = TokenService.GenerateRefreshToken();
        TokenService.DeleteRefreshToken(username, refreshToken);
        TokenService.SaveRefreshToken(username, newRefreshToken);

        return new ObjectResult(new
        {
            token = newJwt,
            refreshToken = newRefreshToken
        });
    }
}