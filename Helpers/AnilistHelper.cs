namespace AnilistExt.Helpers;

public sealed class AnilistHelper
{
    public static event EventHandler? AuthenticationChanged;
    private static readonly Lazy<AnilistHelper> _instance = new Lazy<AnilistHelper>(() => new AnilistHelper());

    public static AnilistHelper Instance { get { return _instance.Value; } }

    public AniClient client;
    public User authedUser;

    private AnilistHelper()
    {
        Log.Logger.Information("AnilistHelper init start");
        client = new AniClient();
        Log.Logger.Information("AnilistHelper init done");
        if (!string.IsNullOrEmpty(AnilistExt.AppSettings.AccessToken))
        {
            client.TryAuthenticateAsync(AnilistExt.AppSettings.AccessToken);
            authedUser = client.GetAuthenticatedUserAsync().GetAwaiter().GetResult();
        }
    }

    public async Task UpdateToken(string token)
    {
        if (!string.IsNullOrWhiteSpace(token))
        {
            await client.TryAuthenticateAsync(token);
            authedUser = await client.GetAuthenticatedUserAsync();
            Log.Logger.Information("AnilistHelper re-authenticated");
            Log.Logger.Information($"AnilistHelper authed user set {authedUser}");
            AuthenticationChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}