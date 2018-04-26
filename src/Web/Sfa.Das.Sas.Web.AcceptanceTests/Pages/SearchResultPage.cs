using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Sfa.Das.Sas.Web.AcceptanceTests.Pages.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Das.Sas.Web.AcceptanceTests.Pages
{
    internal class SearchResultPage : FatBasePage
    {
        protected override string PageTitle => "Search Results - Find apprenticeship training";

        public SearchResultPage(IWebDriver webDriver) : base(webDriver)
        {
        }

        public IWebElement Heading => GetByClass("h1.heading-xlarge");
        public IWebElement FirstStandardResult => GetByCss("#apprenticeship-results .standard-result a");
        public IWebElement FirstFrameworkResult => GetByCss("#apprenticeship-results .framework-result a");
        public IWebElement SortingDropdown => GetById("select-order");
        public IWebElement FilterBlock => GetByClass("filters-block");
        public IWebElement FilterBlockButton => GetByCss(".filter-box .button");

        internal void SelectLevel(int level)
        {
            var checkbox = GetById($"SelectedLevels_{level}");
            checkbox.Click();
            FilterBlockButton.Click();
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
        }

        internal IEnumerable<ApprenticeshipResultItem> GetAllResults()
        {
            var results = GetAllByClass("result");
            return results
                .Select(m => 
                new ApprenticeshipResultItem {
                    Title = m.FindElement(By.CssSelector(".result-title")).Text,
                    Level = m.FindElement(By.ClassName("level")).Text
                }
            );
        }

        internal void SortBy(string v)
        {
            var el = new SelectElement(SortingDropdown);
            el.SelectByText(v);
        }
    }
}