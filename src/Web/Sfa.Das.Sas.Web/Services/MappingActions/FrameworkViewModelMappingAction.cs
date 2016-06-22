using AutoMapper;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Web.Services.MappingActions.Helpers;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Services.MappingActions
{
    using System.Linq;

    public class FrameworkViewModelMappingAction : IMappingAction<Framework, FrameworkViewModel>
    {
        public void Process(Framework source, FrameworkViewModel destination)
        {
            destination.TypicalLengthMessage = ApprenticeshipMappingHelper.GetTypicalLengthMessage(source.TypicalLength);
            destination.JobRoles = source.JobRoleItems?.Select(m => m.Title);
            destination.ExpiryDateString = source.ExpiryDate?.AddDays(1).ToString("d MMMM yyyy");
        }
    }
}