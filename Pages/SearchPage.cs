using System;
using System.Linq;
using System.Threading;
using AnilistExt.Commands;
using Serilog;

namespace AnilistExt;

internal sealed partial class SearchPage : DynamicListPage
{
    public override ICommandItem? EmptyContent { get; set; }
    private IListItem[] _results = [];
    private string _lastQuery = string.Empty;
    private readonly AnilistMediaFilters mediaFilters;
    
    public SearchPage()
    {
        Log.Logger.Information("SearchPage start: init");
        Title = "Search";
        Name = "Open";
        PlaceholderText = "Search anime, manga, characters or users.";
        EmptyContent = new CommandItem(new NoOpCommand())
        {
            Title = "Start typing to search.",
            Icon = IconHelpers.FromRelativePath("Assets\\AniListlogo.png")
        };
        mediaFilters = new AnilistMediaFilters();
        mediaFilters.PropChanged += MediaFiltersOnPropChanged;
        Filters = mediaFilters;
        ShowDetails = true;
    }

    private void MediaFiltersOnPropChanged(object sender, IPropChangedEventArgs args)
    {
        IsLoading = true;
        Task.Run(() => FetchMedia(_lastQuery));
    }
    private CancellationTokenSource? _cts;
    public override void UpdateSearchText(string oldSearch, string newSearch)
    {
        _lastQuery = newSearch;
        
        if (string.IsNullOrWhiteSpace(newSearch))
        {
            _cts?.Cancel();
            _results = [];
            IsLoading = false;
            RaiseItemsChanged();
            return;
        }
        
        _cts?.Cancel();
        _cts = new CancellationTokenSource();
        var token = _cts.Token;
       
        IsLoading = true;
        Task.Run(async () =>
        {
            try
            {
                await Task.Delay(300, token);
                if (!token.IsCancellationRequested)
                {
                   await FetchMedia(newSearch);
                }
            }
            catch (Exception e)
            {
                Log.Logger.Error(e.Message);
            }
        }, token);
    }
    
    private async Task FetchMedia(string query)
    {
        if (string.IsNullOrEmpty(query)) return;
        
        if (!AnilistHelper.Instance.client.IsAuthenticated)
        {
            _results = [
                new ListItem(new CommandItem(new SaveCredsPage()))
                {
                    Title = "Not authenticated",
                    Subtitle = "Click here to set your AniList token",
                }
            ];
            IsLoading = false;
            RaiseItemsChanged();
            return;
        }
        
        try
        {
            var mediaType = mediaFilters.SelectedMediaType;
            
            var fetchedResults = await AnilistHelper.Instance.client.SearchMediaAsync(new SearchMediaFilter
            {
                Query = query,
                Type = mediaType,
                
            }).ConfigureAwait(false);
            
            Log.Logger.Information($"Response received. Data null? {fetchedResults?.Data == null}, count: {fetchedResults?.Data?.Length}");
            if (query != _lastQuery)
            {
                Log.Logger.Information($"Response received. Data null? {fetchedResults?.Data == null}, count: {fetchedResults?.Data?.Length}");
                return;
            }
            
            
            _results = fetchedResults.Data
                .Select(m => (IListItem)new ListItem(new OpenUrlCommand($"https://anilist.co/{(m.Type == MediaType.Anime ? "anime" : "manga")}/{m.Id}/{m.Title.PreferredTitle}"))
                    {
                        Title = m.Title.PreferredTitle,
                        Subtitle = $"{m.Format} • {m.Status} • {m.AverageScore}%",
                        Icon = new IconInfo(m.Cover.LargeImageUrl.AbsoluteUri),
                        Details = new Details()
                        {
                            HeroImage =  new IconInfo(m.Cover.ExtraLargeImageUrl.AbsoluteUri),
                            Title = m.Title.PreferredTitle,
                            Body = m.Description,
                            Metadata = [
                                new DetailsElement()  
                                {  
                                    Key = "Format",  
                                    Data = new DetailsLink() { Text = $"{m.Format}"},
                                },  
                                new DetailsElement()
                                {
                                    Key = "Genres",
                                    Data = new DetailsTags()
                                    {
                                        Tags = (m.Genres ?? []).Select(g => new Tag(g)).ToArray()
                                    }
                                },
                                new DetailsElement()  
                                {  
                                    Key = "Status",  
                                    Data = new DetailsLink() { Text = $"{m.Status}"},
                                },  
                                new DetailsElement()  
                                {  
                                    Key = "Start Date",  
                                    Data = new DetailsLink()
                                    {
                                        Text = m.StartDate is { Year: not null, Month: not null }
                                            ? $"{m.StartDate.Year}/{m.StartDate.Month:D2}/{m.StartDate.Day:D2}"
                                            : "Unknown"
                                    },
                                },  
                                m.Type == MediaType.Anime ? new DetailsElement()  
                                {  
                                    Key = "Season",  
                                    Data = new DetailsLink() { Text = $"{m.Season}"},
                                } : new DetailsElement()  
                                {  
                                    Key = "",  // Empty cause mangas don't have seasons, not an error
                                    Data = new DetailsLink() { Text = ""},
                                },  
                                new DetailsElement()  
                                {  
                                    Key = "Average Score",  
                                    Data = new DetailsLink() { Text = $"{m.AverageScore}%"},
                                },  
                                new DetailsElement()  
                                {  
                                    Key = "Mean Score",  
                                    Data = new DetailsLink() { Text = $"{m.MeanScore}%"},
                                },  
                                new DetailsElement()  
                                {  
                                    Key = "Popularity",  
                                    Data = new DetailsLink() { Text = $"{m.Popularity}"},
                                },  
                                new DetailsElement()  
                                {  
                                    Key = "Favorites",  
                                    Data = new DetailsLink() { Text = $"{m.Favorites}"},
                                },
                            ]
                        },
                        
                        MoreCommands = [
                            new CommandContextItem(new NoOpCommand()) { Title = "Add to List", Command = new AddToListPage(m)},
                            new CommandContextItem(new NoOpCommand()) { Title = "Remove from List", Command = new RemoveFromUserList(m.Id)},
                            // new CommandContextItem(new NoOpCommand()) { Title = "+1"},
                            // new CommandContextItem(new NoOpCommand()) { Title = ""},
                            // new CommandContextItem(new NoOpCommand()) { Title = ""},
                            new CommandContextItem(new OpenUrlCommand($"https://anilist.co/{(m.Type == MediaType.Anime ? "anime" : "manga")}/{m.Id}/{m.Title.PreferredTitle}")) { Title = "Open in browser."},
                        ]
                    })
                .ToArray() ?? []; 
            
            Log.Logger.Information($"_results set to {_results.Length} items");
        }
        catch (AniException ex)
        {
            Log.Logger.Information(ex, "FetchManga");
            _results = [new ListItem(new NoOpCommand()) { Title = $"{ex.Message}." }];
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex, "Unexpected error in FetchMedia");
            _results = [new ListItem(new NoOpCommand()) { Title = $"Error: {ex.Message}" }];
        }
        finally
        {
            Log.Logger.Information($"Finally: raising items changed, count={_results.Length}");
            IsLoading = false;
            RaiseItemsChanged();
        }
        
    }
    
    public override IListItem[] GetItems()
    {
        return _results;
    }
}