using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Infrastructure.Elasticsearch;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Services
{
    [TestFixture]
    public class ProviderLocationProcessingServicesTests
    {
        private const int ExpectedNumberOf100Percent = 6;
        private const int ExpectedNumberOfDayRelease = 4;
        private const int ExpectedNumberOfBlockRelease = 7;
        private ProviderLocationProcessingService _providerLocationProcessingService;
        private List<FrameworkProviderSearchResultsItem> _documents;
        private FrameworkProviderSearchResultsItem _documentAllThreeAndIsNational;
        private FrameworkProviderSearchResultsItem _documentDayRelease;
        private FrameworkProviderSearchResultsItem _documentBlockRelease;
        private FrameworkProviderSearchResultsItem _document100Percent;
        private FrameworkProviderSearchResultsItem _documentDayReleaseAndIsNational;
        private FrameworkProviderSearchResultsItem _documentBlockReleaseAndIsNational;
        private FrameworkProviderSearchResultsItem _document100PercentAndIsNational;

        [OneTimeSetUp]
        public void Setup()
        {
            _providerLocationProcessingService = new ProviderLocationProcessingService();
            _documents = new List<FrameworkProviderSearchResultsItem>();
            _document100Percent = new FrameworkProviderSearchResultsItem {DeliveryModes = new List<string> {"100PercentEmployer" } };
            var document100PercentAndDayRelease = new FrameworkProviderSearchResultsItem { DeliveryModes = new List<string> { "100PercentEmployer", "DayRelease" } };
            _documentAllThreeAndIsNational = new FrameworkProviderSearchResultsItem { Ukprn = 11111111,   NationalProvider = true, DeliveryModes = new List<string> { "100PercentEmployer", "DayRelease", "BlockRelease" } };
            var document100PercentAndBlockRelease = new FrameworkProviderSearchResultsItem { DeliveryModes = new List<string> { "100PercentEmployer", "BlockRelease" } };
            _documentBlockRelease = new FrameworkProviderSearchResultsItem { DeliveryModes = new List<string> { "BlockRelease" } };
            _documentDayRelease = new FrameworkProviderSearchResultsItem { DeliveryModes = new List<string> { "DayRelease" } };
            var documentDayReleaseLowerCase = new FrameworkProviderSearchResultsItem { DeliveryModes = new List<string> { "dayrelease" } };
            var documentIsNational = new FrameworkProviderSearchResultsItem { NationalProvider = true };
            _documentDayReleaseAndIsNational = new FrameworkProviderSearchResultsItem { NationalProvider = true, DeliveryModes = new List<string> { "DayRelease" } };
            _documentBlockReleaseAndIsNational = new FrameworkProviderSearchResultsItem { NationalProvider = true, DeliveryModes = new List<string> { "BlockRelease" } };
            _document100PercentAndIsNational = new FrameworkProviderSearchResultsItem { NationalProvider = true, DeliveryModes = new List<string> { "100PercentEmployer" } };

            _documents.Add(_document100Percent);
            _documents.Add(_document100Percent);
            _documents.Add(document100PercentAndDayRelease);
            _documents.Add(_documentAllThreeAndIsNational);
            _documents.Add(document100PercentAndBlockRelease);
            _documents.Add(_documentBlockRelease);
            _documents.Add(_documentBlockRelease);
            _documents.Add(_documentBlockRelease);
            _documents.Add(_documentBlockRelease);
            _documents.Add(_documentDayRelease);
            _documents.Add(documentDayReleaseLowerCase);
            _documents.Add(documentIsNational);
            _documents.Add(documentIsNational);
            _documents.Add(_documentDayReleaseAndIsNational);
            _documents.Add(_documentBlockReleaseAndIsNational);
            _documents.Add(_document100PercentAndIsNational);
        }

        [Test]
        public void ShouldSetTheExpectedCountsForTrainingOptionsInTheReturnedDictionary()
        {
            var actual = _providerLocationProcessingService.RetrieveTrainingOptionsAggregationElements(_documents);
            var res = new Dictionary<string, long?> { { "100percentemployer", ExpectedNumberOf100Percent }, { "dayrelease", ExpectedNumberOfDayRelease }, { "blockrelease", ExpectedNumberOfBlockRelease } };

            res.ShouldAllBeEquivalentTo(actual);
        }

        [Test]
        public void ShouldSetTheExpectedCountsForNationalAndNonNationalInTheReturnedDictionary()
        {
            var actual = _providerLocationProcessingService.RetrieveNationalProvidersAggregationElements(_documents);
            var res = new Dictionary<string, long?> { { "1", 6 }, { "0", 10 } };

            res.ShouldAllBeEquivalentTo(actual);
        }

        [Test]
        public void ShouldReturnOnlyDocumentsWithNationalSet()
        {
            var filter = new ProviderSearchFilter {SearchOption = ProviderFilterOptions.ApprenticeshipLocationWithNationalProviderOnly };

            var actual = _providerLocationProcessingService.FilterProviderSearchResults(_documents, filter);

            Assert.AreEqual(6, actual.Count);
            Assert.Contains(_documentAllThreeAndIsNational, actual);
        }

        [Test]
        public void ShouldReturnOnlyDocumentsWith100PercentSet()
        {
            var filter = new ProviderSearchFilter { SearchOption = ProviderFilterOptions.ApprenticeshipId, DeliveryModes = new List<string> { "100percentemployer" } };

            var actual = _providerLocationProcessingService.FilterProviderSearchResults(_documents, filter);

            Assert.AreEqual(ExpectedNumberOf100Percent, actual.Count);
            Assert.Contains(_document100Percent, actual);
            Assert.Contains(_documentAllThreeAndIsNational, actual);
        }

        [Test]
        public void ShouldReturnDocumentsWith100PercentAndDayReleaseSet()
        {
            var filter = new ProviderSearchFilter { SearchOption = ProviderFilterOptions.ApprenticeshipId, DeliveryModes = new List<string> { "100percentemployer", "dayrelease" } };

            var actual = _providerLocationProcessingService.FilterProviderSearchResults(_documents, filter);

            Assert.AreEqual(8, actual.Count);
            Assert.Contains(_document100Percent, actual);
            Assert.Contains(_documentAllThreeAndIsNational, actual);
            Assert.Contains(_documentDayRelease, actual);
        }

        [Test]
        public void ShouldReturnDocumentsWith100PercentAndBlockReleaseSet()
        {
            var filter = new ProviderSearchFilter { SearchOption = ProviderFilterOptions.ApprenticeshipId, DeliveryModes = new List<string> { "100percentemployer", "blockrelease" } };

            var actual = _providerLocationProcessingService.FilterProviderSearchResults(_documents, filter);

            Assert.AreEqual(11, actual.Count);
            Assert.Contains(_document100Percent, actual);
            Assert.Contains(_documentAllThreeAndIsNational, actual);
            Assert.Contains(_documentBlockRelease, actual);
        }

        [Test]
        public void ShouldReturnDocumentsWith100PercentBlockReleaseAndDayReleaseSet()
        {
            var filter = new ProviderSearchFilter { SearchOption = ProviderFilterOptions.ApprenticeshipId, DeliveryModes = new List<string> { "100percentemployer", "blockrelease", "dayrelease" } };

            var actual = _providerLocationProcessingService.FilterProviderSearchResults(_documents, filter);

            Assert.AreEqual(13, actual.Count);
            Assert.Contains(_document100Percent, actual);
            Assert.Contains(_documentAllThreeAndIsNational, actual);
            Assert.Contains(_documentBlockRelease, actual);
            Assert.Contains(_documentDayRelease, actual);
        }

        [Test]
        public void ShouldReturnDocumentsWithBlockReleaseAndDayReleaseSet()
        {
            var filter = new ProviderSearchFilter { SearchOption = ProviderFilterOptions.ApprenticeshipId, DeliveryModes = new List<string> { "blockrelease", "dayrelease" } };

            var actual = _providerLocationProcessingService.FilterProviderSearchResults(_documents, filter);

            Assert.AreEqual(10, actual.Count);
            Assert.Contains(_documentAllThreeAndIsNational, actual);
            Assert.Contains(_documentBlockRelease, actual);
            Assert.Contains(_documentDayRelease, actual);
        }

        [Test]
        public void ShouldReturnDocumentsWithBlockReleaseSet()
        {
            var filter = new ProviderSearchFilter { SearchOption = ProviderFilterOptions.ApprenticeshipId, DeliveryModes = new List<string> { "blockrelease" } };

            var actual = _providerLocationProcessingService.FilterProviderSearchResults(_documents, filter);

            Assert.AreEqual(ExpectedNumberOfBlockRelease, actual.Count);
            Assert.Contains(_documentAllThreeAndIsNational, actual);
            Assert.Contains(_documentBlockRelease, actual);
        }

        [Test]
        public void ShouldReturnDocumentsWithDayReleaseSet()
        {
            var filter = new ProviderSearchFilter { SearchOption = ProviderFilterOptions.ApprenticeshipId, DeliveryModes = new List<string> { "dayrelease" } };

            var actual = _providerLocationProcessingService.FilterProviderSearchResults(_documents, filter);

            Assert.AreEqual(ExpectedNumberOfDayRelease, actual.Count);
            Assert.Contains(_documentAllThreeAndIsNational, actual);
            Assert.Contains(_documentDayRelease, actual);
        }

        [Test]
        public void ShouldReturnDocumentsWithDayReleaseAndIsNationalSet()
        {
            var filter = new ProviderSearchFilter { SearchOption = ProviderFilterOptions.ApprenticeshipLocationWithNationalProviderOnly, DeliveryModes = new List<string> { "dayrelease" } };

            var actual = _providerLocationProcessingService.FilterProviderSearchResults(_documents, filter);

            Assert.AreEqual(2, actual.Count);
            Assert.Contains(_documentAllThreeAndIsNational, actual);
            Assert.Contains(_documentDayReleaseAndIsNational, actual);
        }

        [Test]
        public void ShouldReturnDocumentsWith100PercentAndIsNationalSet()
        {
            var filter = new ProviderSearchFilter { SearchOption = ProviderFilterOptions.ApprenticeshipLocationWithNationalProviderOnly, DeliveryModes = new List<string> { "100percentemployer" } };

            var actual = _providerLocationProcessingService.FilterProviderSearchResults(_documents, filter);

            Assert.AreEqual(2, actual.Count);
            Assert.Contains(_documentAllThreeAndIsNational, actual);
            Assert.Contains(_document100PercentAndIsNational, actual);
        }

        [Test]
        public void ShouldReturnDocumentsWithBlockReleaseAndIsNationalSet()
        {
            var filter = new ProviderSearchFilter { SearchOption = ProviderFilterOptions.ApprenticeshipLocationWithNationalProviderOnly, DeliveryModes = new List<string> { "blockrelease" } };

            var actual = _providerLocationProcessingService.FilterProviderSearchResults(_documents, filter);

            Assert.AreEqual(2, actual.Count);
            Assert.Contains(_documentAllThreeAndIsNational, actual);
            Assert.Contains(_documentBlockReleaseAndIsNational, actual);
        }

        [Test]
        public void ShouldReturnDocumentsWithBlockReleaseDayReleaseAndIsNationalSet()
        {
            var filter = new ProviderSearchFilter { SearchOption = ProviderFilterOptions.ApprenticeshipLocationWithNationalProviderOnly, DeliveryModes = new List<string> { "blockrelease", "dayrelease"} };

            var actual = _providerLocationProcessingService.FilterProviderSearchResults(_documents, filter);

            Assert.AreEqual(3, actual.Count);
            Assert.Contains(_documentAllThreeAndIsNational, actual);
            Assert.Contains(_documentBlockReleaseAndIsNational, actual);
            Assert.Contains(_documentDayReleaseAndIsNational, actual);
        }

        [Test]
        public void ShouldCastDocumentsOfStandardTypeToStandard()
        {
            var documents = new List<IApprenticeshipProviderSearchResultsItem> { new StandardProviderSearchResultsItem() };
            var actual = _providerLocationProcessingService.CastDocumentsToMatchingResultsItemType<StandardProviderSearchResultsItem>(documents);
            documents.ShouldAllBeEquivalentTo(actual);
        }

        [Test]
        public void ShouldCastDocumentsOfFrameworkTypeToFramework()
        {
            var documents = new List<IApprenticeshipProviderSearchResultsItem> { new FrameworkProviderSearchResultsItem() };
            var actual = _providerLocationProcessingService.CastDocumentsToMatchingResultsItemType<FrameworkProviderSearchResultsItem>(documents);
            documents.ShouldAllBeEquivalentTo(actual);
        }

        [Test]
        public void ShouldCastDocumentsThatDoNotMatchFrameworkOrStandardToNull()
        {
            var documents = new List<IApprenticeshipProviderSearchResultsItem> {new TestClass()};
            var actual = _providerLocationProcessingService.CastDocumentsToMatchingResultsItemType<TestClass>(documents);
            Assert.IsNull(actual);
        }



        private class TestClass : IApprenticeshipProviderSearchResultsItem
        {
            public string ContactUsUrl { get; set; }
            public List<string> DeliveryModes { get; set; }
            public List<string> DeliveryModesKeywords { get; set; }
            public double Distance { get; set; }
            public string Email { get; set; }
            public bool NationalProvider { get; set; }
            public double? EmployerSatisfaction { get; set; }
            public double? LearnerSatisfaction { get; set; }
            public double? OverallAchievementRate { get; set; }
            public string MarketingName { get; set; }
            public string ProviderMarketingInfo { get; set; }
            public string ApprenticeshipMarketingInfo { get; set; }
            public string ProviderName { get; set; }
            public string LegalName { get; set; }
            public string Phone { get; set; }
            public string ApprenticeshipInfoUrl { get; set; }
            public int Ukprn { get; set; }
            public bool IsHigherEducationInstitute { get; set; }
            public string Website { get; set; }
            public IEnumerable<TrainingLocation> TrainingLocations { get; set; }
            public int? MatchingLocationId { get; set; }
            public double? NationalOverallAchievementRate { get; set; }
            public string OverallCohort { get; set; }
            public bool HasNonLevyContract { get; set; }
            public bool HasParentCompanyGuarantee { get; set; }
            public bool IsNew { get; set; }
        }
    }
}
