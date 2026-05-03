// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AnilistExt;

public partial class AnilistExtCommandsProvider : CommandProvider
{
    private readonly ICommandItem[] _commands;

    public AnilistExtCommandsProvider()
    {
        DisplayName = "Anilist";
        Id = "com.diekoma.anilistextension";
        Icon = IconHelpers.FromRelativePath("Assets\\AniListlogo.png");
        _commands = [
            new CommandItem(new AnilistExtPage()) { Title = DisplayName }
        ];
    }

    public override ICommandItem[] TopLevelCommands()
    {
        return _commands;
    }
    
    //Coming in the next update
    // public override ICommandItem[]? GetDockBands()
    // {
    //     
    //     var profileButton = new ListItem(new AniUserProfilePage())
    //     {
    //         Title = "Profile",
    //         Icon = IconHelpers.FromRelativePath("Assets\\AniListlogo.png")
    //     };
    //     // var airingButton= new ListItem(new NoOpCommand())
    //     // {
    //     //     Title = "Currently Airing",
    //     //     Icon = IconHelpers.FromRelativePath("Assets\\AniListlogo.png")
    //     // };
    //     
    //     List<ICommandItem> bands =
    //     [
    //         profileButton,
    //         //airingButton
    //     ];
    //     
    //     return bands.ToArray();
    // }
}
