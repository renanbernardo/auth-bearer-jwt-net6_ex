using System.IdentityModel.Tokens.Jwt;
using System.Text;
using ShopApi.Interfaces.Settings;
using ShopApi.Models;

namespace ShopApi.Services;

public class TokenService
{
    private readonly ISettingsService _settingsService;

public TokenService(ISettingsService settingsService)
{
    _settingsService = settingsService;
}

    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_settingsService.GetAuthSettings().Secret);
        return "";        
    }
}