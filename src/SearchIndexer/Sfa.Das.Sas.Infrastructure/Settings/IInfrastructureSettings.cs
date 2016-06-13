using System;
using System.Collections.Generic;

namespace Sfa.Das.Sas.Indexer.Infrastructure.Settings
{
    public interface IInfrastructureSettings
    {
        string CourseDirectoryUri { get; }

        string Environment { get; }

        IEnumerable<Uri> ElasticServerUrls { get; }
    }
}