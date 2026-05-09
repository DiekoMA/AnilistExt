namespace AnilistExt.Pages
{
    internal sealed partial class AnilistSettingsPage : ContentPage
    {
        public AnilistSettingsPage()
        {
            Name = "Settings";
            Icon = new IconInfo("\uE713");
            Title = "Extension Settings";
        }

        public override IContent[] GetContent()
        {
            return AnilistExt.AppSettings.Settings.ToContent();
        }
    }
}
