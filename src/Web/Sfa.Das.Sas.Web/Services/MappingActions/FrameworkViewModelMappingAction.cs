using System;
using AutoMapper;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Web.Services.MappingActions.Helpers;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Services.MappingActions
{
    public class FrameworkViewModelMappingAction : IMappingAction<Framework, FrameworkViewModel>
    {
        public void Process(Framework source, FrameworkViewModel destination)
        {
            destination.TypicalLengthMessage = StandardMappingHelper.GetTypicalLengthMessage(source.TypicalLength);
        }
    }
}