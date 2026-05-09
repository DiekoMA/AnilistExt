namespace AnilistExt.Pages;

internal sealed partial class MediaPreviewPage : ContentPage
{
    private readonly MediaPreviewForm previewForm;
    public override IContent[] GetContent()
    {
        return [new MediaPreviewForm()];
    }
}


internal sealed partial class MediaPreviewForm : FormContent
{
    public MediaPreviewForm()
    {
        TemplateJson = $$"""
                        {
                "type": "AdaptiveCard",
                "$schema": "https://adaptivecards.io/schemas/adaptive-card.json",
                "version": "1.6",
                "body": [
                    {
                        "type": "ColumnSet",
                        "columns": [
                            {
                                "type": "Column",
                                "width": "stretch",
                                "items": [
                                    {
                                        "type": "ColumnSet",
                                        "columns": [
                                            {
                                                "type": "Column",
                                                "width": "stretch",
                                                "items": [
                                                    {
                                                        "type": "Image",
                                                        "url": "https://s4.anilist.co/file/anilistcdn/media/anime/cover/large/bx147105-rwOX8qyUy8gV.jpg"
                                                    }
                                                ]
                                            },
                                            {
                                                "type": "Column",
                                                "width": "stretch",
                                                "items": [
                                                    {
                                                        "type": "TextBlock",
                                                        "text": "Witch Hat Atelier",
                                                        "size": "ExtraLarge",
                                                        "weight": "Bolder",
                                                        "fontType": "Default",
                                                        "style": "columnHeader",
                                                        "height": "stretch"
                                                    }
                                                ]
                                            }
                                        ]
                                    },
                                    {
                                        "type": "TextBlock",
                                        "text": "In a world where everyone takes wonders like magic spells and dragons for granted, Coco is a girl with a simple dream: she wants to be a witch. But everybody knows magicians are born, not made, and Coco was not born with a gift for magic. Resigned to her un-magical life, Coco is about to give up on her dream to become a witch … until the day she meets Qifrey, a mysterious, traveling magician. After secretly seeing Qifrey perform magic in a way she’s never seen before, Coco soon learns what everybody “knows” might not be the truth, and discovers that her magical dream may not be as far away as it may seem...\n",
                                        "wrap": true
                                    }
                                ]
                            }
                        ]
                    }
                ],
                "speak": "Anime/Manga Page"
            }
            """;
    }
}