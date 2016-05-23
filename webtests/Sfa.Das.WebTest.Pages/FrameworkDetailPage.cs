namespace Sfa.Das.WebTest.Pages
{
    using OpenQA.Selenium;

    using Sfa.Das.WebTest.Infrastructure;

    [PageNavigation("/apprenticeship/framework")]
    public class FrameworkDetailsPage : SharedTemplatePage
    {
        public By PostcodeSearchBox => By.CssSelector("#search-box");
        
        public By SearchPageButton => By.CssSelector("a.button");

        public By SearchButton => By.CssSelector("input.button");

    }
}