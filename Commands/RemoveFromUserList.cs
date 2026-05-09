namespace AnilistExt.Commands;

internal sealed partial class RemoveFromUserList : InvokableCommand
{
    private int _mediaId;
    public RemoveFromUserList(int mediaId)
    {
        _mediaId = mediaId;
        Log.Logger.Error("Loaded media with id ${_mediaId}", _mediaId);
    }
    public override CommandResult Invoke()
    {
        try
        {
            Task.Run(async () =>
            {
                Log.Logger.Error("Removing media with id ${_mediaId}", _mediaId);
                await AnilistHelper.Instance.client.DeleteMediaEntryAsync(_mediaId);
            });
        }
        catch (AniException ex)
        {
            Log.Logger.Error(ex.Message);
            Console.WriteLine(ex.Message);
        }
        catch (Exception e)
        {
            Log.Logger.Error(e.Message);
        }
        return CommandResult.ShowToast("Item removed");
    }
}