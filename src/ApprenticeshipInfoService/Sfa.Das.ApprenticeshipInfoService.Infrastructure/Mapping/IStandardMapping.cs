namespace Sfa.Das.ApprenticeshipInfoService.Infrastructure.Mapping
{
    using Sfa.Das.ApprenticeshipInfoService.Core.Models;

    public interface IStandardMapping
    {
        Standard MapToStandard(StandardSearchResultsItem document);
    }
}
