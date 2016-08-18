namespace Sfa.Das.ApprenticeshipInfoService.Infrastructure.Mapping
{
    using Sfa.Das.ApprenticeshipInfoService.Core.Models;

    public interface IFrameworkMapping
    {
        Framework MapToFramework(FrameworkSearchResultsItem document);
    }
}
