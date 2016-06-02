using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Das.Sas.WebTest.Infrastructure.Hooks
{
    using System.Collections.ObjectModel;

    using OpenQA.Selenium;

    using Sfa.Das.Sas.WebTest.Infrastructure.Selenium;

    using SpecBind.Actions;
    using SpecBind.BrowserSupport;
    using SpecBind.Pages;

    public class TestPostNavigateHook : NavigationPostAction
    {
        private readonly IBrowser _browser;

        public TestPostNavigateHook(IBrowser browser)
        {
            _browser = browser;
        }

        protected override void OnPageNavigate(IPage page, PageNavigationAction.PageAction actionType, IDictionary<string, string> pageArguments)
        {
            CheckForJavascriptErrors();
        }

        public void CheckForJavascriptErrors()
        {
            var errors = ((IJavaScriptExecutor)_browser.Driver()).ExecuteScript("return window.jsErrors") as ReadOnlyCollection<object> ?? new ReadOnlyCollection<object>(new List<object>());
            if (errors.Any())
            {
                throw new ApplicationException(string.Join("\n", errors.Select(x => x.ToString())));
            }
        }
    }
}
