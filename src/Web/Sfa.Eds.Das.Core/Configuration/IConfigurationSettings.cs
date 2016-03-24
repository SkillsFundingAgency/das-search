namespace Sfa.Eds.Das.Core.Configuration
{
    using System;
    using System.Collections.Generic;

    public interface IConfigurationSettings
    {
        string SearchHost { get; }

        string ApprenticeshipIndexAlias { get; }

        string ProviderIndexAlias { get; }

        string ElasticServerIp { get; }

        string BuildId { get; }

        IEnumerable<Uri> ElasticServerIps { get; }
    }
}