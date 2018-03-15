using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Infrastructure.Elasticsearch;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Services
{
    [TestFixture]
    public class DeduplicationServiceTests
    {
        private DeduplicationService _deduplicationService;

        [OneTimeSetUp]
        public void Setup()
        {
            _deduplicationService = new DeduplicationService();
        }

        [Test]
        public void ShouldHaveNoEffectOnDifferentFrameworksWithDifferentDetails()
        {
             var frameworks = new List<FrameworkProviderSearchResultsItem>();
            var expectedframeworks = new List<FrameworkProviderSearchResultsItem>();
            var framework1 = new FrameworkProviderSearchResultsItem { Ukprn = 11111111, DeliveryModes = new List<string> { "dayrelease", "blockrelease", "100percentemployer" } };
            var framework2 = new FrameworkProviderSearchResultsItem { Ukprn = 22222222, DeliveryModes = new List<string> { "blockrelease", "100percentemployer" } };
            var framework3 = new FrameworkProviderSearchResultsItem { Ukprn = 33333333,  };

            frameworks.Add(framework1);
            frameworks.Add(framework2);
            frameworks.Add(framework3);

            expectedframeworks.Add(framework1);
            expectedframeworks.Add(framework2);
            expectedframeworks.Add(framework3);
            var actual = _deduplicationService.DedupeAtYourLocationOnlyDocuments(frameworks);

            actual.ShouldAllBeEquivalentTo(expectedframeworks);
        }


        [Test]
        public void ShouldHaveNoEffectOnFrameworksSameUkprnsButNotOnlyAtOneLocation()
        {
            var frameworks = new List<FrameworkProviderSearchResultsItem>();
            var expectedframeworks = new List<FrameworkProviderSearchResultsItem>();
            var framework1 = new FrameworkProviderSearchResultsItem { Ukprn = 11111111, DeliveryModes = new List<string> { "dayrelease", "blockrelease", "100percentemployer" } };
            var framework2 = new FrameworkProviderSearchResultsItem { Ukprn = 11111111, DeliveryModes = new List<string> { "blockrelease", "100percentemployer" } };
            var framework3 = new FrameworkProviderSearchResultsItem { Ukprn = 11111111 };

            frameworks.Add(framework1);
            frameworks.Add(framework2);
            frameworks.Add(framework3);

            expectedframeworks.Add(framework1);
            expectedframeworks.Add(framework2);
            expectedframeworks.Add(framework3);
            var actual = _deduplicationService.DedupeAtYourLocationOnlyDocuments(frameworks);

            actual.ShouldAllBeEquivalentTo(expectedframeworks);
        }

        [Test]
        public void ShouldHaveNoEffectOnFrameworksDifferentUkprnsButOnlyAtOneLocation()
        {
            var frameworks = new List<FrameworkProviderSearchResultsItem>();
            var expectedframeworks = new List<FrameworkProviderSearchResultsItem>();
            var framework1 = new FrameworkProviderSearchResultsItem { Ukprn = 11111111, DeliveryModes = new List<string> { "100percentemployer" } };
            var framework2 = new FrameworkProviderSearchResultsItem { Ukprn = 22222222, DeliveryModes = new List<string> { "100percentemployer" } };
            var framework3 = new FrameworkProviderSearchResultsItem { Ukprn = 33333333, DeliveryModes = new List<string> { "100percentemployer" } };

            frameworks.Add(framework1);
            frameworks.Add(framework2);
            frameworks.Add(framework3);

            expectedframeworks.Add(framework1);
            expectedframeworks.Add(framework2);
            expectedframeworks.Add(framework3);
            var actual = _deduplicationService.DedupeAtYourLocationOnlyDocuments(frameworks);

            actual.ShouldAllBeEquivalentTo(expectedframeworks);
        }

        [Test]
        public void ShouldHaveEffectOnFrameworksDuplicateUkprnsButOnlyAtOneLocation()
        {
            var frameworks = new List<FrameworkProviderSearchResultsItem>();
            var expectedframeworks = new List<FrameworkProviderSearchResultsItem>();
            var framework1 = new FrameworkProviderSearchResultsItem { Ukprn = 11111111, DeliveryModes = new List<string> { "100PercentEmployer" } };
            var framework2 = new FrameworkProviderSearchResultsItem { Ukprn = 22222222, DeliveryModes = new List<string> { "100PercentEmployer" } };
            var framework3 = new FrameworkProviderSearchResultsItem { Ukprn = 33333333, DeliveryModes = new List<string> { "100PercentEmployer" } };

            frameworks.Add(framework1);
            frameworks.Add(framework2);
            frameworks.Add(framework3);
            frameworks.Add(framework1);
            frameworks.Add(framework1);
            frameworks.Add(framework2);

            expectedframeworks.Add(framework1);
            expectedframeworks.Add(framework2);
            expectedframeworks.Add(framework3);
            var actual = _deduplicationService.DedupeAtYourLocationOnlyDocuments(frameworks);

            actual.ShouldAllBeEquivalentTo(expectedframeworks);
        }


        [Test]
        public void ShouldHaveNoEffectOnDifferentStandardsWithDifferentDetails()
        {
            var standards = new List<StandardProviderSearchResultsItem>();
            var expectedStandards = new List<StandardProviderSearchResultsItem>();
            var standard1 = new StandardProviderSearchResultsItem { Ukprn = 11111111, DeliveryModes = new List<string> { "dayrelease", "blockrelease", "100percentemployer" } };
            var standard2 = new StandardProviderSearchResultsItem { Ukprn = 22222222, DeliveryModes = new List<string> { "blockrelease", "100percentemployer" } };
            var standard3 = new StandardProviderSearchResultsItem { Ukprn = 33333333, };

            standards.Add(standard1);
            standards.Add(standard2);
            standards.Add(standard3);

            expectedStandards.Add(standard1);
            expectedStandards.Add(standard2);
            expectedStandards.Add(standard3);
            var actual = _deduplicationService.DedupeAtYourLocationOnlyDocuments(standards);

            actual.ShouldAllBeEquivalentTo(expectedStandards);
        }

        [Test]
        public void ShouldHaveNoEffectOnStandardsSameUkprnsButNotOnlyAtOneLocation()
        {
            var standards = new List<StandardProviderSearchResultsItem>();
            var expectedStandards = new List<StandardProviderSearchResultsItem>();
            var standard1 = new StandardProviderSearchResultsItem { Ukprn = 11111111, DeliveryModes = new List<string> { "dayrelease", "blockrelease", "100percentemployer" } };
            var standard2 = new StandardProviderSearchResultsItem { Ukprn = 11111111, DeliveryModes = new List<string> { "blockrelease", "100percentemployer" } };
            var standard3 = new StandardProviderSearchResultsItem { Ukprn = 11111111 };

            standards.Add(standard1);
            standards.Add(standard2);
            standards.Add(standard3);

            expectedStandards.Add(standard1);
            expectedStandards.Add(standard2);
            expectedStandards.Add(standard3);
            var actual = _deduplicationService.DedupeAtYourLocationOnlyDocuments(standards);

            actual.ShouldAllBeEquivalentTo(expectedStandards);
        }

        [Test]
        public void ShouldHaveNoEffectOnStandardsDifferentUkprnsButOnlyAtOneLocation()
        {
            var standards = new List<StandardProviderSearchResultsItem>();
            var expectedStandards = new List<StandardProviderSearchResultsItem>();
            var standard1 = new StandardProviderSearchResultsItem { Ukprn = 11111111, DeliveryModes = new List<string> { "100percentemployer" } };
            var standard2 = new StandardProviderSearchResultsItem { Ukprn = 22222222, DeliveryModes = new List<string> { "100percentemployer" } };
            var standard3 = new StandardProviderSearchResultsItem { Ukprn = 33333333, DeliveryModes = new List<string> { "100percentemployer" } };

            standards.Add(standard1);
            standards.Add(standard2);
            standards.Add(standard3);

            expectedStandards.Add(standard1);
            expectedStandards.Add(standard2);
            expectedStandards.Add(standard3);
            var actual = _deduplicationService.DedupeAtYourLocationOnlyDocuments(standards);

            actual.ShouldAllBeEquivalentTo(expectedStandards);
        }

        [Test]
        public void ShouldHaveEffectOnStandardsDuplicateUkprnsButOnlyAtOneLocation()
        {
            var standards = new List<StandardProviderSearchResultsItem>();
            var expectedStandards = new List<StandardProviderSearchResultsItem>();
            var standard1 = new StandardProviderSearchResultsItem { Ukprn = 11111111, DeliveryModes = new List<string> { "100PercentEmployer" } };
            var standard2 = new StandardProviderSearchResultsItem { Ukprn = 22222222, DeliveryModes = new List<string> { "100PercentEmployer" } };
            var standard3 = new StandardProviderSearchResultsItem { Ukprn = 33333333, DeliveryModes = new List<string> { "100PercentEmployer" } };

            standards.Add(standard1);
            standards.Add(standard2);
            standards.Add(standard3);
            standards.Add(standard1);
            standards.Add(standard1);
            standards.Add(standard2);

            expectedStandards.Add(standard1);
            expectedStandards.Add(standard2);
            expectedStandards.Add(standard3);
            var actual = _deduplicationService.DedupeAtYourLocationOnlyDocuments(standards);

            actual.ShouldAllBeEquivalentTo(expectedStandards);
        }
    }
}
