using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Shared.Components.Domain.Interfaces;
using Sfa.Das.Sas.Shared.Components.Mapping;
using Sfa.Das.Sas.Shared.Components.ViewComponents.Fat;
using Sfa.Das.Sas.Shared.Components.ViewComponents.Fat.SearchResults;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Das.Sas.Shared.Components.ViewModels;

namespace Sfa.Das.Sas.Shared.Components.UnitTests.ViewComponents.Fat
{
    [TestFixture]
    public class FatSearchResultsViewComponentTests : ViewComponentTestsBase
    {
        private FatSearchResultsViewComponent _sut;
        private Mock<IApprenticeshipSearchService> _apprenticeshipSearchServicetMock;
        private Mock<IFatSearchResultsViewModelMapper> _FatResultsViewModelMock;

        private  SearchQueryViewModel _searchQueryViewModel = new SearchQueryViewModel();

        private ApprenticeshipSearchResults _searchResults = new ApprenticeshipSearchResults();
        private FatSearchResultsViewModel _searchResultsViewModel = new FatSearchResultsViewModel();

        [SetUp]
        public void Setup()
        {
            base.Setup();

            _apprenticeshipSearchServicetMock = new Mock<IApprenticeshipSearchService>(MockBehavior.Strict);
            _FatResultsViewModelMock = new Mock<IFatSearchResultsViewModelMapper>(MockBehavior.Strict);

            _apprenticeshipSearchServicetMock.Setup(s => s.SearchByKeyword(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<int>>())).Returns(_searchResults);

            _FatResultsViewModelMock.Setup(s => s.Map(_searchResults, It.IsAny<ICssClasses>())).Returns(_searchResultsViewModel);

            _sut = new FatSearchResultsViewComponent(_cssClasses.Object, _apprenticeshipSearchServicetMock.Object, _FatResultsViewModelMock.Object)
            {
                ViewComponentContext = _viewComponentContext
            };
        }

        [Test]
        public async Task Then_FatSearchResultsViewModel_Is_Returned()
        {
            _searchQueryViewModel.Keywords = "keyword";

            var result = await _sut.InvokeAsync(_searchQueryViewModel) as ViewViewComponentResult;

            result.Should().BeOfType<ViewViewComponentResult>();

            result.ViewData.Model.Should().BeOfType<FatSearchResultsViewModel>();
        }

        [Test]
        public async Task Then_Apprenticeships_Are_Searched_By_Keyword()
        {
            _searchQueryViewModel.Keywords = "keyword";

            var result = await _sut.InvokeAsync(_searchQueryViewModel) as ViewViewComponentResult;

            _apprenticeshipSearchServicetMock.Verify(s => s.SearchByKeyword(_searchQueryViewModel.Keywords, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<int>>()));
        }
        [Test]
        public async Task Then_Search_Results_Are_Mapped_To_ViewModel()
        {
            _searchQueryViewModel.Keywords = "keyword";

            var result = await _sut.InvokeAsync(_searchQueryViewModel) as ViewViewComponentResult;

            result.Should().BeOfType<ViewViewComponentResult>();

            _FatResultsViewModelMock.Verify(v => v.Map(_searchResults,It.IsAny<ICssClasses>()));


        }
    }
}
