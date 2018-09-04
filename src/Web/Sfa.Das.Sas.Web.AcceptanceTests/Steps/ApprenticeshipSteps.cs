using System;
using FluentAssertions;
using Sfa.Das.Sas.Web.AcceptanceTests.Pages;
using TechTalk.SpecFlow;

namespace Sfa.Das.Sas.Web.AcceptanceTests
{
    [Binding]
    public class ApprenticeshipSteps : StepsBase
    {
        [When(@"I chooes the first result")]
        public void WhenIChooesTheFirstResult()
        {
            var searchResultPage = Get<ApprenticeshipSearchResultPage>();
            var apprenticeshipPage = searchResultPage.ClickOnResult(1);
            Set(apprenticeshipPage);
        }

        [Then(@"I see the apprenticeship page with heading '(.*)'")]
        public void ThenISeeAPageWithHeading(string heading)
        {
            var page = Get<ApprenticeshipPage>();

            page.Heading.Text.Should().Be(heading);
        }

        [Then(@"I see the apprenticeship page with level '(.*)'")]
        public void ThenISeeTheApprenticeshipPageWithLevel(string level)
        {
            var page = Get<ApprenticeshipPage>();

            page.Level.Text.Should().Be(level);
        }

        [Then(@"I see the apprenticeship page with Typical length '(.*)'")]
        public void ThenISeeTheApprenticeshipPageWithTypicalLength(string typicalLenght)
        {
            var page = Get<ApprenticeshipPage>();

            page.TypicalLength.Text.Should().Be(typicalLenght);
        }

        [Then(@"I click on Find training providers")]
        public void ThenIClickOnFindTrainingProvidersGa_Find_Provider_Bottom()
        {
            var page = Get<ApprenticeshipPage>();

            var postCodeSearchaPage = page.SearchForProvidersClick();
            Set(postCodeSearchaPage);
        }

        // Search Provider
        [Then(@"I am on the Find a training provider page")]
        public void ThenIAmOnTheFindATrainingProviderPage()
        {
            var page = Get<PostCodeSearchPage>();

            page.IsCurrentPage.Should().BeTrue();
        }

        [Then(@"I add postcode '(.*)'")]
        public void ThenIAddPostcode(string postCode)
        {
            var page = Get<PostCodeSearchPage>();
            page.SearchBox.SendKeys(postCode);
            page.LevyPaying.Click();
        }

        [Then(@"I click the search button")]
        public void ThenIClickTheSearchButton()
        {
            var page = Get<PostCodeSearchPage>();
            Set(page.ClickSearchButton());
        }
    }
}