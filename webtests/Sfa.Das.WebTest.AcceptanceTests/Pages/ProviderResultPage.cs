namespace Sfa.Das.WebTest.AcceptanceTests.Pages
{
    using System;
    using System.Linq;

    using NUnit.Framework;

    using OpenQA.Selenium;

    using Sfa.Das.WebTest.AcceptanceTests;

    class ProviderResultPage : BasePage
    {
        /// <summary>
        /// Purpose of this class is to 
        /// Create and maintain all Provider Page objects 
        /// Create and maintain business methods  associated to this page.
        /// </summary>
        SearchPage srchPage;

        By providerlist = By.XPath(".//*[@id='provider-results']/div[1]/div[2]/p");

        By providerlist1 = By.XPath(".//*[@id='provider-results']/div[1]/article/header/h2/a");

        By selectprovider = By.XPath(".//*[@id='provider-results']/div[1]/article[1]/header/h2/a");

        By providerwebsite = By.XPath(".//*[@id='provider-results']/div[1]/article[1]/dl/dt[2]");

        By providereSatisfaction = By.XPath(".//*[@id='provider-results']/div[1]/article[8]/dl/dt[3]");

        By providerlSatisfaction = By.XPath(".//*[@id='provider-results']/div[1]/article[8]/dl/dt[4]");

        By providerLocation = By.XPath(".//*[@id='provider-results']/div[1]/article[3]/dl/dd[2]");

        By resultItems = By.CssSelector("#provider-results .result");

        private By resultsContainer = By.Id("provider-results");

        public void WaitToLoad()
        {
            base.WaitFor(resultsContainer);
        }

        public void verifyProviderResultsPage()
        {
            Assert.True(FindElements(resultItems).Any(ElementIsDisplayed));
        }

        public void verifyProvidersearchResultsInfo(String info)
        {
            switch (info)
            {
                case "website":
                    Assert.True(isDisplayed(providerwebsite));
                    break;

                case "Employer satisfaction":
                    Assert.True(isDisplayed(providereSatisfaction));
                    break;
                case "Learner satisfaction":
                    Assert.True(isDisplayed(providerlSatisfaction));
                    break;
            }

            AssertIsDisplayed(providerlist);
        }

        public void chooseProvider()
        {
            click(selectprovider);
        }

        public void verifyProviderinSearchResults(string p0)
        {
            AssertIsElementPresent(providerlist1, p0);
        }

        public void verifyProviderLocationinSearchResults(String p0)
        {
            AssertContainsText(providerLocation, p0);
        }

        public void verifyProviderNotinSearchResults(String p0)
        {
            AssertIsElementNotPresent(providerlist1, p0);
        }
    }
}