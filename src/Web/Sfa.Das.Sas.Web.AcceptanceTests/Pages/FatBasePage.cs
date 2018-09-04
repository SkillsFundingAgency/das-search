using System;
using OpenQA.Selenium;
using Sfa.Automation.Framework.Selenium;
using System.Collections.Generic;
using Sfa.Automation.Framework.Enums;

namespace Sfa.Das.Sas.Web.AcceptanceTests.Pages
{
    internal abstract class FatBasePage : BasePage
    {
        protected abstract string PageTitle { get; }
        public abstract bool Verify();

        public IWebElement Heading => GetByCss("h1.heading-xlarge");
        
        protected FatBasePage(IWebDriver webDriver) : base(webDriver)
        {
        }

        public bool IsCurrentPage => Driver.Title == PageTitle;


        protected IWebElement GetById(string selector)
        {
            return Driver.FindElement(By.Id(selector));
        }

        protected IWebElement GetByClass(string selector)
        {
            return Driver.FindElement(By.ClassName(selector));
        }

        protected IEnumerable<IWebElement> GetAllByClass(string selector)
        {
            return Driver.FindElements(By.ClassName(selector));
        }

        protected IWebElement GetByCss(string selector)
        {
            return Driver.FindElement(By.CssSelector(selector));
        }
    }
}