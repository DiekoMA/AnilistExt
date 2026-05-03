// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using AnilistExt.Helpers;
using AniListNet.Objects;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using Serilog;

namespace AnilistExt;

internal sealed partial class AnilistExtPage : ListPage
{
    private string _cachedAvatarPath;
    User aniUser = new();
    bool isAuthed = AnilistHelper.Instance.client.IsAuthenticated;
    public AnilistExtPage()
    {
        _ = SetAuthedUser();
        Icon = IconHelpers.FromRelativePath("Assets\\AniListlogo.png");
        Title = "Anilist";
        Name = "Open";
    }

    private async Task SetAuthedUser()
    {
        if (!isAuthed)
        {
            aniUser = await AnilistHelper.Instance.client.GetAuthenticatedUserAsync();
        } 
    }
    
    public override IListItem[] GetItems()
    {
        Log.Logger.Information("EXTENSION PAGE start: init");
        Log.Logger.Information("EXTENSION PAGE: {isAuthed}", isAuthed);
        Log.Logger.Information("EXTENSION PAGE: logged in as {aniUser.Name}", aniUser.Name);
        
        return [
            isAuthed ? new ListItem(new SaveCredsPage()) { Icon = IconHelpers.FromRelativePath("Assets\\AniListlogo.png"),Title = "Not logged in, click to set your token." }:
                new ListItem(new NoOpCommand()) { Icon = new IconInfo(aniUser.Avatar.LargeImageUrl.AbsoluteUri), Title = $"Logged in as {aniUser.Name}",  Subtitle = "Use the Dock band to view your full profile."},
            new ListItem(new CommandItem(new SearchPage())) { Title = "Search", Subtitle = "Search" },
        ];
    }
}
