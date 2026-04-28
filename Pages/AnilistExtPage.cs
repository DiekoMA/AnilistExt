// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using AnilistExt.Commands;
using AniListNet;
using AniListNet.Objects;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace AnilistExt;

internal sealed partial class AnilistExtPage : ListPage
{
    bool isLoggedIn  = false;
    private string loginUrl = "https://anilist.co/api/v2/oauth/authorize?client_id=40249&response_type=token";
    AniClient client;
    User aniUser;
    public AnilistExtPage()
    {
        client = new AniClient();
        isLoggedIn = client.IsAuthenticated;
        Icon = IconHelpers.FromRelativePath("Assets\\AniListlogo.png");
        Title = "Anilist";
        Name = "Open";
    }

    private async Task Authenticate()
    {
        //await client.TryAuthenticateAsync();
        aniUser = await client.GetAuthenticatedUserAsync();
    }

    public override IListItem[] GetItems()
    {
        var openLoginCommand = new OpenUrlCommand(loginUrl);
        var openProfileCommand = new A_ShowProfile();
        return [
            // First Item should be your user profile
            new ListItem(new NoOpCommand()) { Title = isLoggedIn ? "AUTHENTICATED USERNAME" : aniUser.Name, Subtitle = "100 Manga Read", Command = isLoggedIn ? openProfileCommand : openLoginCommand },
            new ListItem(new NoOpCommand()) { Title = "MalificentMelon",Subtitle = "100 Manga Read"},
            new ListItem(new NoOpCommand()) { Title = "Search", Subtitle = "Search anime"},
            new ListItem(new NoOpCommand()) { Title = "Manga", Subtitle = "Search manga"},
        ];
    }
}
