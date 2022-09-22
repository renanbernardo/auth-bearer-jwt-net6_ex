using Microsoft.Extensions.Options;
using ShopApi.Interfaces.Settings;
using ShopApi.Models.Settings;

namespace ShopApi.Services;

public class SettingsService : ISettingsService
{
    private readonly IOptions<AuthSettings> _authSettingsOptions;

    public SettingsService(IOptions<AuthSettings> authSettingsOptions)
    {
        _authSettingsOptions = authSettingsOptions;
    }

    public AuthSettings GetAuthSettings()
    {
        return _authSettingsOptions.Value;
    }
}