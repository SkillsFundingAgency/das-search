namespace Sfa.Eds.Das.Indexer.Core.Models.Provider
{
    using System.Collections.Generic;

    public interface IApprenticeshipInformation
    {
        ContactInformation ContactInformation { get; set; }

        string MarketingInfo { get; set; }

        string InfoUrl { get; set; }

        IEnumerable<DeliveryInformation> DeliveryLocations { get; set; }

        int Code { get; set; }
    }
}