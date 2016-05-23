namespace Sfa.Das.WebTest.Pages
{
    using OpenQA.Selenium;

    using Sfa.Das.WebTest.Infrastructure;

    [PageNavigation("/apprenticeship/standard")]
    public class StandardDetailsPage : SharedTemplatePage
    {
        public By PostcodeSearchBox => By.CssSelector("#search-box");

        public By SearchPageButton => By.CssSelector("a.button");

        public By SearchButton => By.CssSelector("input.button");
    
        public By ShortlistLink => By.ClassName("shortlist-link");
    }
}