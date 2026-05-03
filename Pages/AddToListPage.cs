using System;
using System.Text.Json.Nodes;
using Serilog;

namespace AnilistExt;

internal sealed partial class AddToListPage : ContentPage
{
    //private readonly AddToListContentForm contentForm;
    private static Media _currentMedia = new();
    private readonly Media _media;
    public override IContent[] GetContent()
    {
        _currentMedia = _media;
        Log.Logger.Information("GetContent called with media id: {id}", _currentMedia.Id);
        return [new AddToListContentForm(_media)];
    }


    public AddToListPage(Media currentMedia)
    {
        _media = currentMedia;
        _currentMedia = currentMedia;
        Log.Logger.Information("AddToListPage constructor called with media id: {id}", currentMedia.Id);
        //contentForm = new AddToListContentForm(_currentMedia);
        Name = "Add to list";
        Title = "Add To List";
        Icon = new IconInfo("\uECA5");
    }
}


internal sealed partial class AddToListContentForm : FormContent
{
    private readonly Media _currentMedia;
    public AddToListContentForm(Media currentMedia)
    {
        _currentMedia = currentMedia;
        TemplateJson = $$"""
                         {
                             "type": "AdaptiveCard",
                             "$schema": "https://adaptivecards.io/schemas/adaptive-card.json",
                             "version": "1.6",
                             "body": [
                                 {
                                     "type": "Input.ChoiceSet",
                                     "label": "Status",
                                     "choices": [
                                         {
                                             "title": "Watching",
                                             "value": "watching"
                                         },
                                         { 
                                             "title": "Plan to watch",
                                             "value": "plan_to_watch"
                                         },
                                         {
                                             "title": "Completed",
                                             "value": "completed"
                                         },
                                         {
                                             "title": "Rewatching",
                                             "value": "rewatching"
                                         },
                                         {
                                             "title": "Paused",
                                             "value": "paused"
                                         },
                                         {
                                             "title": "Dropped",
                                             "value": "dropped"
                                         }
                                     ],
                                     "placeholder": "Status",
                                     "id": "status"
                                 },
                                 {
                                     "type": "Input.Number",
                                     "label": "Score",
                                     "placeholder": "0",
                                     "min": 0,
                                     "max": 10,
                                     "id": "score"
                                 },
                                 {
                                     "type": "Input.Number",
                                     "label": "Episode Progress",
                                     "placeholder": "12",
                                     "max": 100,
                                     "min": 1,
                                     "id": "epprogress"
                                 },
                                 {
                                     "type": "Input.Date",
                                     "label": "Start Date",
                                     "id": "startdate"
                                 },
                                 {
                                     "type": "Input.Date",
                                     "label": "Finish Date",
                                     "id": "finishdate"
                                 }
                             ],
                             "actions": [
                                 {
                                     "type": "Action.Submit",
                                     "title": "Add to List"
                                 }
                             ]
                         }
                         """;
        Log.Logger.Information("TemplateJson: {json}", TemplateJson);
    }
    

    public override CommandResult SubmitForm(string payload)
    {
        var formInput = JsonNode.Parse(payload)?.AsObject();
        if (formInput == null)
        {
            return CommandResult.GoHome();
        }
        var status = (string)formInput["status"]?.ToString() ?? "Watching";
        _ = int.TryParse(formInput["score"].ToString(), out int score);
        _ = int.TryParse(formInput["epprogress"].ToString(), out int progress);
        DateTime.TryParse((string?)formInput["startdate"], out var startDate);
        DateTime.TryParse((string?)formInput["finishdate"], out var finishDate);
        
        ConfirmationArgs confirmArgs = new()
        {
            PrimaryCommand = new AnonymousCommand(
                () =>
                {
                    Log.Logger.Information("Media ${mediaId}", _currentMedia.Id);
                    Task.Run(async () =>
                        {
                            try
                            {
                                await AddEntryToUser(_currentMedia.Id, status, score, progress, startDate, finishDate);
                            }
                            catch (Exception e)
                            {
                                Log.Logger.Error(e.Message);
                            }
                        });
                })
            {
                Name = "Save",
                Result = CommandResult.GoBack(),
            },
            Description = "This will save the entry to your Anilist account",
        };
        return CommandResult.Confirm(confirmArgs);
    }

    private static MediaEntryStatus ParseStatus(string? value) => value switch
    {
        "watching"      => MediaEntryStatus.Current,
        "plan_to_watch" => MediaEntryStatus.Planning,
        "completed"     => MediaEntryStatus.Completed,
        "rewatching"    => MediaEntryStatus.Repeating,
        "paused"        => MediaEntryStatus.Paused,
        "dropped"       => MediaEntryStatus.Dropped,
        _               => throw new ArgumentOutOfRangeException(nameof(value), $"Unknown status: {value}")
    };
    private async Task AddEntryToUser(int mediaId, string status, int score, int progress, DateTime startDate, DateTime endDate)
    {
        await AnilistHelper.Instance.client.SaveMediaEntryAsync(mediaId, new MediaEntryMutation
        {
            Progress = progress,
            Score = score,
            StartDate = startDate,
            CompleteDate = endDate,
            Status = ParseStatus(status),
        });
    }
}
