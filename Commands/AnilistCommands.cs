using Microsoft.CommandPalette.Extensions.Toolkit;

namespace AnilistExt.Commands;

internal sealed partial class AnilistCommands : InvokableCommand
{
    public override CommandResult Invoke()
    {
        
        return CommandResult.KeepOpen();
    }
}

internal sealed partial class A_ShowProfile : InvokableCommand
{
    private async void Authenticate()
    {
        //aniUser = await client.GetAuthenticatedUserAsync();
    }
    
    public override CommandResult Invoke()
    {
        Authenticate();
        return CommandResult.KeepOpen();
    }
}

internal sealed partial class A_SearchManga : InvokableCommand
{
    private async void Authenticate()
    {
        //aniUser = await client.GetAuthenticatedUserAsync();
    }
    
    public override CommandResult Invoke()
    {
        Authenticate();
        return CommandResult.KeepOpen();
    }
}

