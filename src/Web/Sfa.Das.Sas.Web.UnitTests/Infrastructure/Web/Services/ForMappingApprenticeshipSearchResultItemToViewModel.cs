using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Web.Services;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Services
{
    [TestFixture]
    public class ForMappingApprenticeshipSearchResultItemToViewModel
    {
        [TestCase(1, "1 (equivalent to GCSEs at grades D to G)")]
        [TestCase(2, "2 (equivalent to GCSEs at grades A* to C)")]
        [TestCase(3, "3 (equivalent to A levels at grades A to E)")]
        [TestCase(4, "4 (equivalent to certificate of higher education)")]
        [TestCase(5, "5 (equivalent to foundation degree)")]
        [TestCase(6, "6 (equivalent to bachelor's degree)")]
        [TestCase(7, "7 (equivalent to master’s degree)")]
        [TestCase(8, "8 (equivalent to doctorate)")]
        [TestCase(9, "")]
        [TestCase(null, "")]
        public void WhenTypicalLengthIsEmpty(int level, string expected)
        {
            MappingService mappingService = new MappingService(null);

            var apprenticeshipResult = new ApprenticeshipSearchResultsItem
            {
                FrameworkId = 1,
                Level = level,
                FrameworkName = "Framework name",
                PathwayName = "Pathway name",
                JobRoles = new List<string> { "Job role" },
                Keywords = new List<string> { "Keyword" },
                Title = "Title",
                TypicalLength = new TypicalLength { From = 3, To = 6, Unit = "m" }
            };

            var viewModel = mappingService.Map<ApprenticeshipSearchResultsItem, ApprenticeshipSearchResultItemViewModel>(apprenticeshipResult);

            viewModel.Level.Should().Be(expected);
        }
    }
}
