// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace AnilistExt;

internal sealed partial class AnilistExtPage : ListPage
{
    private string _cachedAvatarPath;
    User? aniUser = null;
    bool isAuthed = AnilistHelper.Instance.client.IsAuthenticated;
    public AnilistExtPage()
    {
        _ = SetAuthedUser();
        Icon = IconHelpers.FromRelativePath("Assets\\AniListlogo.png");
        Title = "Anilist";
        Name = "Open";

        AnilistHelper.AuthenticationChanged += OnAuthenticationChanged;
    }

    private void OnAuthenticationChanged(object? sender, EventArgs e)
    {
        Log.Logger.Information("Auth changed, refreshing page");
        aniUser = AnilistHelper.Instance.authedUser;
        RaiseItemsChanged();
    }

    private async Task SetAuthedUser()
    {

        if (!AnilistHelper.Instance.client.IsAuthenticated)
        {
            Log.Logger.Information("Not authenticated, skipping user load");
            return;
        }

        try
        {
            Log.Logger.Information("Fetching authenticated user...");
            aniUser = await AnilistHelper.Instance.client.GetAuthenticatedUserAsync();
            Log.Logger.Information("Got user: {Name}", aniUser?.Name ?? "null");
            RaiseItemsChanged();
        } catch (Exception ex)
        {
            Log.Logger.Error(ex, "Failed to load user");
        }
    }

    public override IListItem[] GetItems()
    {
        try
        {
            Log.Logger.Information("EXTENSION PAGE start: init");
            Log.Logger.Information("EXTENSION PAGE: {isAuthed}", isAuthed);
            Log.Logger.Information("EXTENSION PAGE: logged in as {aniUser.Name}", aniUser?.Name);
            Log.Logger.Information("GetItems: IsAuthenticated={IsAuth}, aniUser null={IsNull}",
                AnilistHelper.Instance.client.IsAuthenticated, aniUser == null);

            if (!AnilistHelper.Instance.client.IsAuthenticated)
            {
                return [
                    new ListItem(new CommandItem(new SaveCredsPage()))
                    {
                        Title = "AniList CMD Pal",
                        Subtitle = "You need to set your AniList token to get started. Click here.",
                        Icon = IconHelpers.FromRelativePath("Assets\\AniListlogo.png")
                    },
                    new ListItem(new OpenUrlCommand("https://github.com/DiekoMA/AnilistExt"))
                    {
                        Title = "٩(◕‿◕｡)۶",
                        Subtitle = "Thanks for installing, if you like the extension show some love by starring the repo" +
                                   "Also leave any issues or feature requests on the github page"
                    }
                ];
            }
            Log.Logger.Information("EXTENSION PAGE start: init");
            Log.Logger.Information("EXTENSION PAGE: {isAuthed}", AnilistHelper.Instance.client.IsAuthenticated);
            if (aniUser == null)
            {
                return [
                    new ListItem(new NoOpCommand()) { Title = "Loading..." },
                ];
            }
            Log.Logger.Information("EXTENSION PAGE: logged in as {aniUser.Name}", aniUser.Name);
            return [
                new ListItem(new NoOpCommand())
                {
                    Icon = aniUser.Avatar?.LargeImageUrl != null
                        ? new IconInfo(aniUser.Avatar.LargeImageUrl.AbsoluteUri)
                        : IconHelpers.FromRelativePath("Assets\\AniListlogo.png"), 
                    Title = $"Logged in as {aniUser.Name}",  
                    Subtitle = "Use the Dock band to view your full profile.",
                    MoreCommands = [
                        new CommandContextItem(new AnilistSettingsPage()) { Title = "Settings", Icon = new IconInfo("\uE713") },
                    ]
                },
                new ListItem(new CommandItem(new SearchPage()))
                {
                    Title = "Search",
                    Subtitle = "Search",
                    Icon = new IconInfo("\uF78B"),
                    MoreCommands = [
                        new CommandContextItem(new AnilistSettingsPage()) { Title = "Settings", Icon = new IconInfo("\uE713") },
                    ]
                }
            ];
        } catch (Exception ex)
        {
            Log.Logger.Error(ex, "GetItems failed");
            return [new ListItem(new NoOpCommand()) { Title = $"Error: {ex.Message}" }];
        }
    }
}
