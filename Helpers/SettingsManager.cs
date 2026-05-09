namespace AnilistExt.Helpers;

public class SettingsManager : JsonSettingsManager
{
    private static readonly string _namespace = "anilist";
    private static string Namespaced(string propertyName) => $"{_namespace}.{propertyName}";
    private readonly ToggleSetting _useAdaptivePreview = new(
        "use_adaptivepreview",
        "Use Adaptive Cards",
        "Use Adaptive Cards to show more information, like Anime,Manga and user Pages",
        false
        );
    private readonly TextSetting _accessToken = new(
        "access_token",
        "AniList Access Token",
        "Your AniList API access token",
        "");

    public string AccessToken => _accessToken.Value ?? string.Empty;
    public bool UseAdaptivePreview => _useAdaptivePreview.Value;

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
        Settings.Add(_useAdaptivePreview);
        LoadSettings();
        Settings.SettingsChanged += (s, a) =>
        {
            var token = a.GetSetting<string>("access_token");
            var useAdaptiveCards = a.GetSetting<bool>("use_adaptivepreview");
            AnilistHelper.Instance.client.TryAuthenticateAsync(token!);
            SaveSettings();
        };
    }
}