namespace Sfa.Das.WebTest.Pages
{
    using OpenQA.Selenium;

    using Sfa.Das.WebTest.Infrastructure;

    [PageNavigation("/apprenticeship/framework")]
    public class FrameworkDetailsPage : SharedTemplatePage
    {
        public By SearchPageButton => By.CssSelector("a.button");
    }
}