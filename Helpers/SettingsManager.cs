namespace AnilistExt.Helpers;

public class SettingsManager : JsonSettingsManager
{
    private static readonly string _namespace = "anilist";
    private static string Namespaced(string propertyName) => $"{_namespace}.{propertyName}";
    private readonly TextSetting _accessToken = new(
        "access_token",
        "AniList Access Token", 
        "Your AniList API access token",
        "");
    
    public string AccessToken => _accessToken.Value ?? string.Empty;

    internal static string SettingsJsonPath()
    {
        var directory = Utilities.BaseSettingsPath("DiekoMA.Anilist");
        Directory.CreateDirectory(directory);

        return Path.Combine(directory, $"{_namespace}.settings.json");
    }
    public SettingsManager()
    {
        FilePath = SettingsJsonPath();
        Settings.Add(_accessToken);
        LoadSettings();
        Settings.SettingsChanged += (s, a) =>
        {
            var token = a.GetSetting<string>("access_token");
            AnilistHelper.Instance.client.TryAuthenticateAsync(token!);
            SaveSettings();
        };
    }
}