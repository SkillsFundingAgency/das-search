namespace Sfa.Das.WebTest.Infrastructure.Selenium
{
    using OpenQA.Selenium;

    public interface IPageContext
    {
        object CurrentPage { get; set; }

        By FindSelector(string propertyName);
        IWebElement FindElement(string propertyName);
    }
}