namespace Sfa.Das.WebTest.Pages
{
    using OpenQA.Selenium;

    using Sfa.Das.WebTest.Infrastructure;

    [PageNavigation("/")]
    public class LandingPage
    {
        public By SearchBox => By.Id("keywords");

        public By SearchButton => By.Id("submit-keywords");
    }
}