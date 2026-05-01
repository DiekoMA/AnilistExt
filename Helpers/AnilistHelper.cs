using System;
using System.IO;
using System.Threading.Tasks;
using AniListNet;
using AniListNet.Objects;
using Microsoft.CommandPalette.Extensions.Toolkit;
using Newtonsoft.Json;
using Serilog;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace AnilistExt.Helpers;

public sealed class AnilistHelper 
{
    private static readonly Lazy<AnilistHelper> _instance = new Lazy<AnilistHelper>(() => new AnilistHelper());
    
    public static AnilistHelper Instance { get { return _instance.Value; } }
    
    public AniClient client;
    private AnilistHelper()
    {
        Log.Logger.Information("AnilistHelper init start");
        client = new AniClient();
        Log.Logger.Information("AnilistHelper init done");
        if (!string.IsNullOrEmpty(AnilistExt.AppSettings.AccessToken))
        {
            client.TryAuthenticateAsync(AnilistExt.AppSettings.AccessToken);
        }
    }

    public async Task UpdateToken(string token)
    {
        if (!string.IsNullOrWhiteSpace(token))
        {
            await client.TryAuthenticateAsync(token);
            Log.Logger.Information("AnilistHelper re-authenticated");
        }
    }
}