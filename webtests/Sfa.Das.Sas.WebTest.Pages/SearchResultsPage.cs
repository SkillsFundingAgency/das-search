namespace Sfa.Das.Sas.WebTest.Pages
{
    using OpenQA.Selenium;

    using Sfa.Das.Sas.WebTest.Pages.Shared;

    using SpecBind.Pages;
    using SpecBind.Selenium;

    [PageNavigation("/Apprenticeship/SearchResults")]
    public class SearchResultsPage : SharedTemplatePage
    {
        [ElementLocator(CssSelector = "#apprenticeship-results .standard-result a")]
        public IWebElement FirstStandardResult { get; set; }

        [ElementLocator(CssSelector = "#apprenticeship-results .framework-result a")]
        public IWebElement FirstFrameworkResult { get; set; }

        [ElementLocator(Id="apprenticeship-results")]
        public IElementList<IWebElement, ApprenticeshipResultItem> ApprenticeshipResults { get; set; }

        [ElementLocator(CssSelector = ".filters-block")]
        public IWebElement FilterBlock { get; set; }

        [ElementLocator(Id = "select-order")]
        public IWebElement SortingDropdown { get; set; }

        [ElementLocator(Id = "SelectedLevels_2")]
        public IWebElement Level2Checkbox { get; set; }

        [ElementLocator(Id = "SelectedLevels_7")]
        public IWebElement Level7Checkbox { get; set; }

        [ElementLocator(CssSelector = ".filters-block .button")]
        public IWebElement UpdateResultsButton { get; set; }

        [ElementLocator(CssSelector = "#apprenticeship-results .standard-result .result-data-list .level")]
        public IWebElement LevelOfTopResult { get; set; }

        [ElementLocator(Class = "result")]
        public class ApprenticeshipResultItem : WebElement
        {
            public ApprenticeshipResultItem(ISearchContext searchContext)
                : base(searchContext)
            {
            }

            [ElementLocator(CssSelector = ".result-title a")]
            public IWebElement Title { get; set; }           

            
        }
    }
}