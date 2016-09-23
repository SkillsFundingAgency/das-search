using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Sfa.Das.ApprenticeshipInfoService.Api.Controllers;
using Sfa.Das.ApprenticeshipInfoService.Api.Helpers;
using Sfa.Das.ApprenticeshipInfoService.Core.Models;
using Sfa.Das.ApprenticeshipInfoService.Core.Services;

namespace Sfa.Das.ApprenticeshipInfoService.UnitTests.Controllers
{
    public class FrameworkControllerTest
    {
        [Test]
        public void GetAllShouldReturnValidListOfFrameworks()
        {
            var mockGetFrameworks = new Mock<IGetFrameworks>();

            mockGetFrameworks.Setup(x => x.GetAllFrameworks()).Returns(new List<FrameworkSummary>());

            FrameworksController sc = new FrameworksController(mockGetFrameworks.Object);

            var frameworkListResponse = sc.Get();

            frameworkListResponse.Should().NotBeNull();
            frameworkListResponse.Should().BeOfType<List<FrameworkSummary>>();
        }

        [Test]
        public void GetByIdShouldReturnValidFramework()
        {
            var mockGetFrameworks = new Mock<IGetFrameworks>();

            mockGetFrameworks.Setup(x => x.GetFrameworkById(It.IsAny<int>())).Returns(new Framework());

            FrameworksController sc = new FrameworksController(mockGetFrameworks.Object);

            var frameworkResponse = sc.GetFramework(1);

            frameworkResponse.Should().NotBeNull();
            frameworkResponse.Should().BeOfType<Framework>();

        }
    }
}
