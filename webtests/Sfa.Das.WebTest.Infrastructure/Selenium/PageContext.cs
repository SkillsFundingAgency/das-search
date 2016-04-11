namespace Sfa.Das.WebTest.Infrastructure.Selenium
{
    using System;
    using System.Configuration;
    using System.Linq;

    using OpenQA.Selenium;

    public class PageContext : IPageContext
    {
        public object CurrentPage { get; set; }

        public By FindSelector(string propertyName)
        {
            if (CurrentPage == null)
            {
                throw new NullReferenceException("Not currently on a page");
            }

            var propertyInfos = CurrentPage.GetType().GetProperties().Where(x => string.Equals(x.Name, propertyName.Replace(" ", string.Empty), StringComparison.CurrentCultureIgnoreCase)).ToList();
            if (!propertyInfos.Any())
            {
                throw new SettingsPropertyNotFoundException($"Couldn't find the property '{propertyName.Replace(" ", string.Empty)}' on the page '{CurrentPage.GetType().Name}'");
            }

            var getter = propertyInfos.Single().GetMethod;
            return getter.Invoke(CurrentPage, null) as By;
    }
}
}