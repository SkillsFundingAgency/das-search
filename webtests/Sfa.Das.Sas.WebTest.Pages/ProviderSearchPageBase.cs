namespace Sfa.Das.Sas.WebTest.Pages
{
    using OpenQA.Selenium;

    using Sfa.Das.Sas.WebTest.Pages.Shared;

    using SpecBind.Pages;

    public abstract class ProviderSearchPageBase : SharedTemplatePage
    {
        private IWebElement _searchButton;

        [ElementLocator(Id = "search-box")]
        public IWebElement PostcodeSearchBox { get; set; }

        [ElementLocator(Class = "postcode-search-button")]
        public IWebElement SearchButton
        {
            get { return _searchButton; }
            set { _searchButton = value; }
        }

        [ElementLocator(CssSelector = ".postcode-form .error-message")]
        public IWebElement ErrorMessage { get; set; }

        [ElementLocator(Id = "levyPaying")]
        public IWebElement YesImLevyPayingEmployer { get; set; }

        [ElementLocator(Id = "notLevyPaying")]
        public IWebElement NoImNotALevyPayingEmployer { get; set; }
    }
}