namespace Sfa.Das.Sas.WebTest.Pages
{
    using OpenQA.Selenium;

    using Sfa.Das.Sas.WebTest.Pages.Shared;

    using SpecBind.Pages;

    public abstract class ProviderSearchPageBase : SharedTemplatePage
    {
        [ElementLocator(Id = "search-box")]
        public IWebElement PostcodeSearchBox { get; set; }

        [ElementLocator(Class = "postcode-search-button")]
        public IWebElement SearchButton { get; set; }


        [ElementLocator(Id = "error-js-postcode-invalid")]
        public IWebElement InvalidFormatPostCodeErrorMessage { get; set; }


        [ElementLocator(Id = "error-postcode-invalid")]
        public IWebElement InvalidPostCodeErrorMessage { get; set; }

        [ElementLocator(Id = "error-postcode-location")]
        public IWebElement PostCodeNotInEnglandErrorMessage { get; set; }

        [ElementLocator(Id = "error-postcode-wales")]
        public IWebElement WalesInformationAboutApprenticeshipsMessage { get; set; }

        [ElementLocator(Id = "error-postcode-northern-ireland")]
        public IWebElement NorthernIrelandInformationAboutApprenticeshipsMessage { get; set; }

        [ElementLocator(Id = "error-postcode-scotland")]
        public IWebElement ScotlandInformationAboutApprenticeshipsMessage { get; set; }

        [ElementLocator(Id = "levyPaying")]
        public IWebElement YesImLevyPayingEmployer { get; set; }

        [ElementLocator(Id = "notLevyPaying")]
        public IWebElement NoImNotLevyPayingEmployer { get; set; }
    }
}