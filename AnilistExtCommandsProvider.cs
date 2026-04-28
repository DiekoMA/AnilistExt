// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace AnilistExt;

public partial class AnilistExtCommandsProvider : CommandProvider
{
    private readonly ICommandItem[] _commands;

    public AnilistExtCommandsProvider()
    {
        DisplayName = "Anilist";
        Icon = IconHelpers.FromRelativePath("Assets\\AniListlogo.png");
        _commands = [
            new CommandItem(new AnilistExtPage()) { Title = DisplayName },
            new CommandItem(new SaveCredsPage()) { Title = "Set Anilist Token" }
        ];
    }

    public override ICommandItem[] TopLevelCommands()
    {
        return _commands;
    }

}
