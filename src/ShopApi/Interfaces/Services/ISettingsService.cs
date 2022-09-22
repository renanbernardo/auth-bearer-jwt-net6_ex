using ShopApi.Models.Settings;

namespace ShopApi.Interfaces.Settings;

public interface ISettingsService
{
    AuthSettings GetAuthSettings();
}