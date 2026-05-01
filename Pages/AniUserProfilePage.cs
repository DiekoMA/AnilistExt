using System.Threading.Tasks;
using AnilistExt.Helpers;
using AniListNet.Objects;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace AnilistExt;

internal sealed partial class AniUserProfilePage : ContentPage
{
    public override IContent[] GetContent()
    {
        return [new AnilistUserForm(aniUser)];
    }

    User aniUser = new();
    bool isAuthed = AnilistHelper.Instance.client.IsAuthenticated;
    
    public AniUserProfilePage()
    {
        Name = "Profile";
        Title = "Your Profile";
        Icon = new IconInfo("\uECA5");
    }
    
    private async Task SetAuthedUser()
    {
        if (!isAuthed)
        {       
            aniUser = await AnilistHelper.Instance.client.GetAuthenticatedUserAsync();
        } 
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
                             "body": [
                                 {
                                     "type": "Container",
                                     "items": [
                                         {
                                             "type": "Image",
                                             "url": "https://s4.anilist.co/file/anilistcdn/user/avatar/large/b576381-w0jRiU1Ci7JQ.jpg",
                                             "altText": "profile image",
                                             "size": "Large"
                                         },
                                         {
                                             "type": "TextBlock",
                                             "text": "{{user.Name}}",
                                             "wrap": true
                                         },
                                         {
                                             "type": "TextBlock",
                                             "text": "I love manga and code and stuff so yeah",
                                             "wrap": true
                                         }
                                     ],
                                     "separator": true,
                                     "horizontalAlignment": "Left",
                                     "roundedCorners": true
                                 }
                             ]
                         }
                         """;
    }
}