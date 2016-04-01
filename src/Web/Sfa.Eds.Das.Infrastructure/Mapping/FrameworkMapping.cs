using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sfa.Das.ApplicationServices.Models;
using Sfa.Eds.Das.Core.Domain.Model;

namespace Sfa.Eds.Das.Infrastructure.Mapping
{
    public class FrameworkMapping : IFrameworkMapping
    {
        public Framework MapToFramework(FrameworkSearchResultsItem document)
        {
            return new Framework
            {
                Title = document.Title,
                Level = document.Level,
                FrameworkCode = document.FrameworkCode,
                FrameworkId = document.FrameworkId,
                FrameworkName = document.FrameworkName,
                PathwayCode = document.PathwayCode,
                PathwayName = document.PathwayName
            };
        }
    }
}
