namespace Sfa.Das.WebTest.Pages
{
    using OpenQA.Selenium;

    using Sfa.Das.WebTest.Infrastructure;

    [PageNavigation("/apprenticeship/standard")]
    public class StandardDetailsPage : SharedTemplatePage
    {
        public By SearchPageButton => By.CssSelector("a.ui-find-training-providers");
        
        public By ShortlistLink => By.ClassName("shortlist-link");
    }
}