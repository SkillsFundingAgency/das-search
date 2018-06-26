using FluentAssertions;
using Sfa.Das.Sas.Web.AcceptanceTests.Pages;
using TechTalk.SpecFlow;

namespace Sfa.Das.Sas.Web.AcceptanceTests
{
    [Binding]
    public class ProviderLocationSearchResultPageSteps : StepsBase
    {
        [Then(@"I am on the Provider Location Search Result Page")]
        public void ThenIAmOnTheProviderSearchResultPage()
        {
            var page = Get<ProviderLocationSearchResultPage>();

            page.IsCurrentPage.Should().BeTrue();
        }

        [Given(@"I have picked course '(.*)' and search for postcode '(.*)'")]
        public void GivenIHavePickedCourseAndSearchForPostcode(string course, string postCode)
        {
            var page = WebSite.NavigateToSearchProviderLocationResultPage(course, postCode);
            Set(page);
        }

        [Then(@"the satisfaction rates are populated")]
        public void ThenTheSatisfactionRatesArePopulated()
        {
            var page = Get<ProviderLocationSearchResultPage>();
            page.EmployerSatisfaction.Text.Should().MatchRegex("^[0-9]{2}%$");
            page.LearnerSatisfaction.Text.Should().MatchRegex("^[0-9]{2}%$");
        }

        [Then(@"the achievement rates are populated")]
        public void ThenTheAchievementRatesArePopulated()
        {
            var page = Get<ProviderLocationSearchResultPage>();

            page.AchievementRate.Text.Should().MatchRegex("^[0-9]{2}%$");
        }

    }
}