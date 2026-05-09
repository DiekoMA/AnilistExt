namespace AnilistExt;

internal sealed partial class SaveCredsPage : ContentPage
{
    private readonly AnilistTokenContentForm tokenForm;

    public override IContent[] GetContent()
    {
        var currentToken = AnilistExt.AppSettings.AccessToken;
        return [new AnilistTokenContentForm(currentToken)];
    }

    public SaveCredsPage()
    {
        Name = "Save";
        Title = "Save Creds";
        Icon = new IconInfo("\uECA5");
    }
}

internal sealed partial class AnilistTokenContentForm : FormContent
{
    private string _loginUrl = "https://anilist.co/api/v2/oauth/authorize?client_id=40249&response_type=token";
    public AnilistTokenContentForm(string currentToken)
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
                                      "text": " Set AniList Token",
                                      "horizontalAlignment": "Center",
                                      "wrap": true,
                                      "style": "heading"
                                  },
                                  {
                                      "type": "Input.Text",
                                      "label": "Token",
                                      "isRequired": true,
                                      "value": "{{currentToken}}",
                                      "placeholder": "Placeholder text",
                                      "errorMessage": "Token is required",
                                      "id": "access_token"
                                  }
                              ],
                              "actions": [
                                  {
                                      "type": "Action.OpenUrl",
                                      "title": "Get User Token",
                                      "url": "{{_loginUrl}}"
                                  },
                                  {
                                      "type": "Action.Submit",
                                      "title": "Save"
                                  }
                              ]
                          }
                          """;

    }

    public override CommandResult SubmitForm(string payload)
    {
        var formInput = JsonNode.Parse(payload)?.AsObject();
        if (formInput == null)
        {
            return CommandResult.GoHome();
        }

        ConfirmationArgs confirmArgs = new()
        {
            PrimaryCommand = new AnonymousCommand(
                () =>
                {
                    AnilistExt.AppSettings.Settings.Update(payload);
                    AnilistExt.AppSettings.SaveSettings();

                    var newToken = AnilistExt.AppSettings.AccessToken;

                    if (!string.IsNullOrEmpty(newToken))
                    {
                        _ = AnilistHelper.Instance.UpdateToken(newToken);
                        ToastStatusMessage t = new("Token saved! You're now authenticated." ?? "Nothing was entered");
                        t.Show();
                    }
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