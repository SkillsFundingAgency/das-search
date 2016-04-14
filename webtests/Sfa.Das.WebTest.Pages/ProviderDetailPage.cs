namespace Sfa.Das.WebTest.Pages
{
    using OpenQA.Selenium;

    using Sfa.Das.WebTest.Infrastructure;

    [PageNavigation("/provider/detail")]
    public class ProviderDetailsPage
    {
        public By ProviderName => By.Id("provider-name");
    }
}