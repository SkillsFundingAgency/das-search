namespace Sfa.Das.WebTest.Pages
{
    using OpenQA.Selenium;

    using Sfa.Das.WebTest.Infrastructure;

    [PageNavigation("/provider/frameworkresults")]
    public class FrameworkProviderResultsPage : SharedTemplatePage
    {
        public By ProviderResults => By.CssSelector("#provider-results article.result");

        public By FirstProviderLink => By.CssSelector("#provider-results article.result:nth-of-type(1) .result-title a");
    }
}