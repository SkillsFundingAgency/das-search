namespace Sfa.Das.ApplicationServices
{
    using Sfa.Das.ApplicationServices.Models;

    public interface IStandardSearchService
    {
        ApprenticeshipSearchResults SearchByKeyword(string keywords, int skip, int take);
    }
}
