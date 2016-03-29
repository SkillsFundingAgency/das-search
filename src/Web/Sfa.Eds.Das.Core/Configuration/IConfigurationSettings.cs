namespace Sfa.Eds.Das.Core.Configuration
{
    using System;
    using System.Collections.Generic;

    public interface IConfigurationSettings
    {
        string ApprenticeshipIndexAlias { get; }

        string ProviderIndexAlias { get; }

        string BuildId { get; }

        IEnumerable<Uri> ElasticServerUrls { get; }
    }
}