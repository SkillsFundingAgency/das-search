using Sfa.Das.Sas.Web.AcceptanceTests.Pages;
using TechTalk.SpecFlow;

namespace Sfa.Das.Sas.Web.AcceptanceTests
{
    public class StepsBase
    {
        protected static WebSite WebSite;
        protected static Config Config;

        public T Get<T>(string key = null) where T : class
        {
            return key == null ? ScenarioContext.Current.Get<T>() : ScenarioContext.Current.Get<T>(key);
        }

        public void Set<T>(T item, string key = null)
        {
            if (key == null)
                ScenarioContext.Current.Set(item);
            else
                ScenarioContext.Current.Set(item, key);
        }
    }
}
