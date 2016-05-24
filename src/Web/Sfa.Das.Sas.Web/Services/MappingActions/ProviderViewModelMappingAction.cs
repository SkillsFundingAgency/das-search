using AutoMapper;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Web.Services.MappingActions.Helpers;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Services.MappingActions
{
    public class ProviderViewModelMappingAction : IMappingAction<ProviderCourse, ProviderCourseViewModel>
    {
        public void Process(ProviderCourse source, ProviderCourseViewModel destination)
        {
            destination.EmployerSatisfactionMessage = ProviderMappingHelper.GetSatisfactionText(source.EmployerSatisfaction);
            destination.LearnerSatisfactionMessage = ProviderMappingHelper.GetSatisfactionText(source.LearnerSatisfaction);
        }
    }
}