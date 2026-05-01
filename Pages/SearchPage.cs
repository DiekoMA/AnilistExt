using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnilistExt.Helpers;
using AniListNet;
using AniListNet.Objects;
using AniListNet.Parameters;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using Serilog;
using Serilog.Core;

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
            Title = """¯\_(ツ)_/¯""",
            Subtitle = "Start typing to search."
        };
        mediaFilters = new AnilistMediaFilters();
        mediaFilters.PropChanged += MediaFiltersOnPropChanged;
        Filters = mediaFilters;
        
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
                .Select(m =>
                    (IListItem)new ListItem(
                        new OpenUrlCommand($"https://anilist.co/{(m.Type == MediaType.Anime ? "anime" : "manga")}/{m.Id}/{m.Title.PreferredTitle}"))
                    {
                        Title = m.Title.PreferredTitle,
                        Subtitle = $"{m.Format} • {m.Status} • {m.AverageScore}%",
                        Icon = new IconInfo(m.Cover.LargeImageUrl.AbsoluteUri)
                    })
                .ToArray() ?? []; 
            
            Log.Logger.Information($"_results set to {_results.Length} items");
        }
        catch (AniException ex)
        {
            Log.Logger.Information(ex, "FetchManga");
            _results = [new ListItem(new NoOpCommand()) { Title = $"{ex.Message}." }];
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