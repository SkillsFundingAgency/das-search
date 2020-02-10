using System;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Infrastructure.Mapping;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Mapping
{

    using ApiFramework = SFA.DAS.Apprenticeships.Api.Types.Framework;

    [TestFixture]
    public class FrameworkMappingTests
    {
        private FrameworkMapping _sut;
        private FrameworkSearchResultsItem _frameworkFromSearchResultsItem;
        private ApiFramework _frameworkApiItem;
        private DateTime _effectiveToDate;
        private DateTime _expectedEffectiveToDate;

        [SetUp]
        public void Setup()
        {
            _effectiveToDate = new DateTime(2000, 1, 1, 0, 0, 0, 0, 0);
            _expectedEffectiveToDate = new DateTime(2000, 1, 2, 0, 0, 0, 0, 0);

            _sut = new FrameworkMapping();
            _frameworkFromSearchResultsItem = new FrameworkSearchResultsItem()
            {
                Title = "Test Framework",
                EffectiveTo = _effectiveToDate
            };

            _frameworkApiItem = new ApiFramework()
            {
                Title = "Test Framework From Api",
                EffectiveTo = _effectiveToDate
            };
        }

        [Test]
        public void When_mapping_framework_from_Framework_search_results_Then_1_day_is_added_to_the_EffectiveTo_date()
        {
            var mappedFramework = _sut.MapToFramework(_frameworkApiItem);

            Assert.AreEqual(_expectedEffectiveToDate, mappedFramework.EffectiveTo);
        }

        [Test]
        public void When_mapping_framework_from_Framework_api_Then_1_day_is_added_to_the_EffectiveTo_date()
        {
            var mappedFramework = _sut.MapToFramework(_frameworkFromSearchResultsItem);

            Assert.AreEqual(_expectedEffectiveToDate, mappedFramework.EffectiveTo);
        }
    }
}
