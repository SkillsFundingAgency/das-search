namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using NUnit.Framework;

    using Sfa.Das.Sas.Core.Domain.Model;
    using Sfa.Das.Sas.Web.Services.MappingActions;
    using Sfa.Das.Sas.Web.ViewModels;

    [TestFixture]
    public class ForMappingFrameworksToViewModel
    {
        [Test]
        public void ShouldGetJobTitlesFromJobRoleItems()
        {
            FrameworkViewModelMappingAction sut = new FrameworkViewModelMappingAction();

            var framework = new Framework
            {
                FrameworkId = 10,
                Level = 2,
                Title = "this is a framework",
                JobRoleItems = new List<JobRoleItem>()
                                   {
                                       new JobRoleItem { Title = "SFA master", Description = "Nothing" },
                                       new JobRoleItem { Title = "DAS master", Description = "Nothing here" }
                                   }
            };
            var resultViewModel = new FrameworkViewModel();
            sut.Process(framework, resultViewModel);

            resultViewModel.JobRoles.ToArray()[0].ShouldBeEquivalentTo("SFA master");
            resultViewModel.JobRoles.ToArray()[1].ShouldBeEquivalentTo("DAS master");
        }
    }
}
