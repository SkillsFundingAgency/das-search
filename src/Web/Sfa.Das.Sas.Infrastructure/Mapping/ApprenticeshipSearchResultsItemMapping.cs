using System.Linq;
using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.Infrastructure.Mapping
{
    using System.Collections.Generic;
    using Sfa.Das.Sas.ApplicationServices.Models;
    public class ApprenticeshipSearchResultsItemMapping : IApprenticeshipSearchResultsItemMapping
    {

        public ApprenticeshipSearchResultsItem Map(SFA.DAS.Apprenticeships.Api.Types.ApprenticeshipSearchResultsItem document)
        {
            if (document != null)
            {
                var item = new ApprenticeshipSearchResultsItem();

                item.FrameworkId = document.FrameworkId;
                item.FrameworkName = document.FrameworkName;
                item.StandardId = document.StandardId;
                item.Duration = document.Duration;
                item.EffectiveFrom = document.EffectiveFrom;
                item.EffectiveTo = document.EffectiveTo;
                item.Level = document.Level;
                item.Published = document.Published;
                item.PathwayName = document.PathwayName;
                item.Title = document.Title;
                item.Keywords = document.Keywords;


                item.JobRoleItems = document.JobRoleItems?.Select(s => new JobRoleItem() { Description = s.Description, Title = s.Title });

                item.JobRoles = document.JobRoles?.Select(s => s).ToList();

                return item;
            }

            return null;
        }
    }
}