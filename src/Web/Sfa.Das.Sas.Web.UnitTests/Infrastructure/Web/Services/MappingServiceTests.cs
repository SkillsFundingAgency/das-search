using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.Core.Logging;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Sfa.Das.Sas.Core.Domain.Model;
    using Sfa.Das.Sas.Web.Services;
    using Sfa.Das.Sas.Web.ViewModels;

    [TestFixture]
    public sealed class MappingServiceTests
    {
        [Test]
        public void MappingConfigurationShouldBeValid()
        {
            var service = new MappingService(Mock.Of<ILog>());

            service.Configuration.AssertConfigurationIsValid();
        }

        [Test]
        public void ShouldMappFromFrameworkFrameworkViewModel()
        {
            var service = new MappingService(Mock.Of<ILog>());
            var framework = new Framework
            {
                ExpiryDate = new DateTime(1882, 09, 04),
                TypicalLength = new TypicalLength { From = 12, To = 18, Unit = "m" },
                JobRoleItems = new List<JobRoleItem> { new JobRoleItem { Description = "Description 1", Title = "Title1" } }
            };

            var viewModel = service.Map<Framework, FrameworkViewModel>(framework);

            viewModel.ExpiryDateString.Should().Be("5 September 1882");
            viewModel.TypicalLengthMessage.Should().Be("12 to 18 months");
            viewModel.JobRoles.FirstOrDefault().Should().Be("Title1");
        }

        [Test]
        public void ShouldMappFromFrameworkFrameworkViewModelWhenFrameworkIsEmpty()
        {
            var service = new MappingService(Mock.Of<ILog>());
            var framework = new Framework
            {
                Title = "title1",
            };

            var viewModel = service.Map<Framework, FrameworkViewModel>(framework);

            viewModel.ExpiryDateString.Should().BeNull();
            viewModel.Should().NotBeNull();
        }
    }
}
