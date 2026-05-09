namespace AnilistExt;

internal sealed partial class AnilistMediaFilters : Filters
{
    // internal const string AllMediaFiltersId = "any";
    internal const string AnimeFilterId = "anime";
    internal const string MangaFilterId = "manga";
    // internal const string CharacterFilterId = "character";

    private readonly List<SearchMediaFilter> _mediaFilters;

    public AnilistMediaFilters()
    {
        CurrentFilterId = AnimeFilterId;
    }

    public MediaType SelectedMediaType => CurrentFilterId switch
    {
        MangaFilterId => MediaType.Manga,
        _ => MediaType.Anime
    };

    public override IFilterItem[] GetFilters() =>
    [
        new Filter { Id = AnimeFilterId, Name = "Anime" },
        new Filter { Id = MangaFilterId, Name = "Manga" },
    ];
}