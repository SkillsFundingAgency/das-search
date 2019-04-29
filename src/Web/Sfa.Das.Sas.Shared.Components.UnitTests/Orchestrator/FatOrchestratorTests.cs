using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Shared.Components.Mapping;
using Sfa.Das.Sas.Shared.Components.Orchestrators;
using Sfa.Das.Sas.Shared.Components.ViewComponents.Fat;
using Sfa.Das.Sas.Shared.Components.ViewModels;

namespace Sfa.Das.Sas.Shared.Components.UnitTests.Orchestrator
{
    [TestFixture]
    public class FatOrchestratorTests
    {
        private FatOrchestrator _sut;
        private Mock<IApprenticeshipSearchService> _apprenticeshipSearchServicetMock;
        private Mock<IFatSearchResultsViewModelMapper> _FatResultsViewModelMock;

        private  SearchQueryViewModel _searchQueryViewModel = new SearchQueryViewModel();

        private ApprenticeshipSearchResults _searchResults = new ApprenticeshipSearchResults();
        private FatSearchResultsViewModel _searchResultsViewModel = new FatSearchResultsViewModel();

        [SetUp]
        public void Setup()
        {

            _apprenticeshipSearchServicetMock = new Mock<IApprenticeshipSearchService>(MockBehavior.Strict);
            _FatResultsViewModelMock = new Mock<IFatSearchResultsViewModelMapper>(MockBehavior.Strict);

            _apprenticeshipSearchServicetMock.Setup(s => s.SearchByKeyword(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<int>>())).Returns(_searchResults);

            _FatResultsViewModelMock.Setup(s => s.Map(_searchResults)).Returns(_searchResultsViewModel);

            _sut = new FatOrchestrator(_apprenticeshipSearchServicetMock.Object, _FatResultsViewModelMock.Object);
        }

        [Test]
        public void Then_FatSearchResultsViewModel_Is_Returned()
        {
            _searchQueryViewModel.Keywords = "keyword";

            var result = _sut.GetSearchResults(_searchQueryViewModel);

            result.Should().BeOfType<FatSearchResultsViewModel>();
        }

        [Test]
        public void Then_Apprenticeships_Are_Searched_By_Keyword()
        {
            _searchQueryViewModel.Keywords = "keyword";

            var result = _sut.GetSearchResults(_searchQueryViewModel);

            _apprenticeshipSearchServicetMock.Verify(s => s.SearchByKeyword(_searchQueryViewModel.Keywords, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<int>>()));
        }
        [Test]
        public void Then_Search_Results_Are_Mapped_To_ViewModel()
        {
            _searchQueryViewModel.Keywords = "keyword";

            var result = _sut.GetSearchResults(_searchQueryViewModel);

            result.Should().BeOfType<FatSearchResultsViewModel>();

            _FatResultsViewModelMock.Verify(v => v.Map(_searchResults));


        }
    }
}
