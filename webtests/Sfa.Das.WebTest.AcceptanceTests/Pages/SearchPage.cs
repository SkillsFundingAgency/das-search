namespace Sfa.Das.WebTest.AcceptanceTests.Pages
{
    using System.Collections.Generic;
    using System.Linq;

    using NUnit.Framework;

    using OpenQA.Selenium;

    using Sfa.Das.WebTest.AcceptanceTests;

    public class SearchPage : BasePage

    {
        private readonly By _searchResultTitles = By.CssSelector("#apprenticeship-results .result .result-title");

        private readonly By firstStandardinresult = By.XPath(".//*[@id='apprenticeship-results']/div[1]/article[1]/header/h2/a");

        private readonly By Invalidsearchmessage = By.XPath(".//*[@id='apprenticeship-results']/div[1]/div[2]/p");

        /// <summary>
        ///     Purpose of this class is to
        ///     Create and maintain all Search Page objects and methods.
        ///     Any changes to search functionality can be maintained under this page.
        /// </summary>

        // private IWebDriver driver;
        private readonly By searchBox = By.Id("keywords");

        private readonly By searchButton = By.Id("submit-keywords");

        private readonly By searchkeywordresult = By.XPath(".//*[@id='apprenticeship-results']/div[1]/article/header/h2/a");

        // Search Results Page
        private readonly By searchresult = By.XPath(".//*[@id='apprenticeship-results']/div[1]/p");

        private readonly By searchResultcount = By.CssSelector(".column-two-thirds>div>p>b");

        private readonly By typicallength = By.XPath(".//*[@id='apprenticeship-results']/div[1]/article/dl/dd[2]");

        public void Navigate()
        {
            base.Navigate("apprenticeship/search");
            WaitForSearchPage();
        }

        public void WaitForSearchPage()
        {
            WaitFor(searchButton);
        }

        public void launchLandingPage()
        {
            Launch("Home Page - Employer Apprenticeship Search");
            Sleep(3000);
        }

        public void OpenStandarDetails(string standard)
        {
            Open(standard);
            // Sleep(3000);
        }

        public void OpenFrameworkDetails(string framework)
        {
            OpenFramework(framework);
            // Sleep(3000);
        }

        public void SearchKeyword(string keyword)
        {
            type(keyword, searchBox);
        }

        public void clickSearchBox()
        {
            click(searchButton);
        }

        public void verifyresultsPages()
        {
            AssertIsDisplayed(searchresult);
        }

        public void verifySearchedStandardFoundinResultPage(string expected_result)
        {
            AssertIsElementPresent(searchkeywordresult, expected_result);
        }

        public void verifyStandardinTopofList(string keyword)
        {
            AssertIsElementPresent(firstStandardinresult, keyword);
        }

        public void VerifyresultCount()
        {
            Assert.True(GetText(searchResultcount).Contains("Total results found:"));
        }

        public void Verifylength()
        {
            AssertContainsText(typicallength, "24 to 36 months");
        }

        public void verifySearchresultMessage(string msg)
        {
            Assert.True(GetText(Invalidsearchmessage).Contains(msg));
        }

        public void Open(string standard)
        {
            driver.Navigate().GoToUrl(baseUrl + "/Apprenticeship/Standard/" + standard);
        }

        public void OpenFramework(string framework)
        {
            driver.Navigate().GoToUrl(baseUrl + "/Apprenticeship/Framework/" + framework);
        }

        public IEnumerable<string> FindSearchResultTitles()
        {
            return FindElements(_searchResultTitles).Select(x => x.Text);
        }
    }
}