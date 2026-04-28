using Microsoft.CommandPalette.Extensions.Toolkit;

namespace AnilistExt.Commands;

internal sealed partial class SearchAnimeCommand : InvokableCommand
{
    public override CommandResult Invoke()
    {
        
        return CommandResult.KeepOpen();
    }
}