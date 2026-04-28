using System.Diagnostics;
using System.IO;
using System.Text.Json.Nodes;
using AniListNet;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace AnilistExt;

internal sealed partial class SaveCredsPage : ContentPage
{
    private readonly AnilistTokenContentForm tokenForm = new();
    public override IContent[] GetContent() => [tokenForm];

    public SaveCredsPage()
    {
        Name = "Save";
        Title = "Save Creds";
        Icon = new IconInfo("\uECA5");
    }
}

internal sealed partial class AnilistTokenContentForm : FormContent
{
    public AnilistTokenContentForm()
    {
        TemplateJson = $$"""
                         {
                             "type": "AdaptiveCard",
                             "$schema": "https://adaptivecards.io/schemas/adaptive-card.json",
                             "version": "1.6",
                             "body": [
                                 {
                                     "type": "TextBlock",
                                     "size": "Medium",
                                     "weight": "Bolder",
                                     "text": " ${TokenForm.title}",
                                     "horizontalAlignment": "Center",
                                     "wrap": true,
                                     "style": "heading"
                                 },
                                 {
                                     "type": "Input.Text",
                                     "label": "Token",
                                     "isRequired": true,
                                     "placeholder": "Placeholder text",
                                     "errorMessage": "Token is required",
                                     "id": "Token"
                                 }
                             ],
                             "actions": [
                                 {
                                     "type": "Action.Submit",
                                     "title": "Save",
                                     "id": "save_button"
                                 }
                             ]
                         }
                         """;

    }

    public override CommandResult SubmitForm(string payload)
    {
        var formInput = JsonNode.Parse(payload)?.AsObject();
        Debug.WriteLine($"Form submitted with formInput: {formInput}");
        if (formInput == null)
        {
            return CommandResult.GoHome();
        }
        
        ConfirmationArgs confirmArgs = new()
        {
            PrimaryCommand = new AnonymousCommand(
                () =>
                {
                    string? token = formInput["token"]?.ToString();
                    File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "secrets", "ani_token.json"), token);
                    ToastStatusMessage t = new($"Saved to filePath" ?? "Nothing was entered");
                    t.Show();
                })
            {
                Name = "Save",
                Result = CommandResult.GoBack(),
            },
            Description = "This will save your Anilist AccessToken for use, your AccessToken lasts for 1 year.",
        };
        return CommandResult.Confirm(confirmArgs);
    }
}