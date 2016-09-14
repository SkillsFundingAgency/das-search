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
            var mockHelper = new Mock<IControllerHelper>();

            mockGetFrameworks.Setup(x => x.GetAllFrameworks()).Returns(new List<FrameworkSummary>());

            FrameworkController sc = new FrameworkController(mockGetFrameworks.Object, mockHelper.Object);

            var frameworkListResponse = sc.Get();

            frameworkListResponse.Should().NotBeNull();
        }

        [Test]
        public void GetAllShouldReturnValidFramework()
        {
            var mockGetFrameworks = new Mock<IGetFrameworks>();
            var mockHelper = new Mock<IControllerHelper>();

            mockGetFrameworks.Setup(x => x.GetFrameworkById(It.IsAny<int>())).Returns(new Framework());

            FrameworkController sc = new FrameworkController(mockGetFrameworks.Object, mockHelper.Object);

            var frameworkResponse = sc.GetFramework(1);

            frameworkResponse.Should().NotBeNull();
        }
    }
}
