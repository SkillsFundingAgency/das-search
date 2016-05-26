namespace Sfa.Das.WebTest.Pages
{
    using OpenQA.Selenium;

    using Sfa.Das.WebTest.Infrastructure;

    [PageNavigation("/apprenticeship/standard")]
    public class StandardDetailsPage : SharedTemplatePage
    {
        public By SearchPageButton => By.CssSelector("a.button");
        
        public By ShortlistLink => By.ClassName("shortlist-link");
    }
}