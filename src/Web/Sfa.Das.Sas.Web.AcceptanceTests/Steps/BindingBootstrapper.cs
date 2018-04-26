using Sfa.Das.Sas.Web.AcceptanceTests.Pages;
using TechTalk.SpecFlow;

namespace Sfa.Das.Sas.Web.AcceptanceTests
{
    [Binding]
    public class BindingBootstrapper : StepsBase
    {

        [BeforeTestRun(Order = 0)]
        public static void SetUpContainer()
        {
            Config = new Config();
        }

        [BeforeScenario(Order = 0)]
        public void SetUpNestedContainer()
        {
            WebSite = new WebSite(Config.WebSiteUrl, Config.Browser);
        }

        [AfterScenario(Order = 999)]
        public void CleanUpNestedContainer()
        {
            WebSite?.Close();
        }
    }
}
