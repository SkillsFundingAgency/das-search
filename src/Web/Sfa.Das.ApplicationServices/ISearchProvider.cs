namespace Sfa.Das.ApplicationServices
{
    using Sfa.Das.ApplicationServices.Models;

    public interface ISearchProvider
    {
        StandardSearchResults SearchByKeyword(string keywords, int skip, int take);
        ProviderSearchResults SearchByStandardId(string standardId, int skip, int take);
    }
}
