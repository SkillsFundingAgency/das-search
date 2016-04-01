namespace Sfa.Infrastructure.Settings
{
    using System;
    using System.Collections.Generic;

    public interface IInfrastructureSettings
    {
        string CourseDirectoryUri { get; }

        IEnumerable<Uri> ElasticServerUrls { get; }
    }
}