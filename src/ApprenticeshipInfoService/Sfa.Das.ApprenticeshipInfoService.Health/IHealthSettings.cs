namespace Sfa.Das.ApprenticeshipInfoService.Health
{
    using System;
    using System.Collections.Generic;

    public interface IHealthSettings
    {
        string Environment { get; }

        IEnumerable<Uri> ElasticsearchUrls { get; }

        string LarsZipFileUrl { get; }

        string CourseDirectoryUrl { get; }
    }
}