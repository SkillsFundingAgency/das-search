namespace Sfa.Das.WebTest.Pages
{
    using OpenQA.Selenium;

    using Sfa.Das.WebTest.Infrastructure;

    [PageNavigation("/apprenticeship/framework")]
    public class FrameworkDetailsPage : SharedTemplatePage
    {
        public By PostcodeSearchBox => By.Id("postcode");

        public By SearchButton => By.Id("submit-postcode");

    }
}