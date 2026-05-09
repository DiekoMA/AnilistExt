namespace AnilistExt;

internal sealed partial class AniUserProfilePage : ContentPage
{
    public override IContent[] GetContent()
    {
        return [new AnilistUserForm(_aniUser)];
    }

    private readonly User _aniUser = new();

    public AniUserProfilePage(User aniUser)
    {
        _aniUser = aniUser;
        Name = "Profile";
        Title = "Your Profile";
        Icon = new IconInfo("\uECA5");
    }
}

internal sealed partial class AnilistUserForm : FormContent
{
    public AnilistUserForm(User user)
    {
        TemplateJson = $$"""
                                                  {
                             "type": "AdaptiveCard",
                             "$schema": "https://adaptivecards.io/schemas/adaptive-card.json",
                             "version": "1.6",
                             "speak": "Anime/Manga Page",
                             "body": [
                                 {
                                     "type": "ColumnSet",
                                     "columns": [
                                         {
                                             "type": "Column",
                                             "width": "stretch",
                                             "items": [
                                                 {
                                                     "type": "Image",
                                                     "url": "{{user.Avatar.LargeImageUrl}}",
                                                     "style": "RoundedCorners",
                                                     "horizontalAlignment": "Left",
                                                     "size": "Stretch"
                                                 }
                                             ]
                                         },
                                         {
                                             "type": "Column",
                                             "width": "stretch",
                                             "items": [
                                                 {
                                                     "type": "TextBlock",
                                                     "text": "{{user.Name}}",
                                                     "wrap": true,
                                                     "weight": "Bolder",
                                                     "size": "ExtraLarge",
                                                     "fontType": "Default",
                                                     "style": "heading"
                                                 },
                                                 {
                                                     "type": "TextBlock",
                                                     "text": "{{user.About}}",
                                                     "wrap": true
                                                 }
                                             ]
                                         }
                                     ]
                                 },
                                 {
                                     "type": "CompoundButton",
                                     "title": "Notice",
                                     "description": "This will change later, after i add some extra functionality to the extension."
                                 }
                             ],
                             "rtl": false,
                             "minHeight": "455px"
                         }
                         """;
    }
}