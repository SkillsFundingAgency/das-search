namespace Sfa.Das.WebTest.Infrastructure.Steps
{
    using System;
    using System.Configuration;
    using System.Linq;

    using TechTalk.SpecFlow;

    [Binding]
    public class DataSteps
    {
        [Given(@"I have data for a (.*)")]
        public void GivenIHaveDataForA(string type)
        {
            foreach (var key in ConfigurationManager.AppSettings.AllKeys.Where(x => x.StartsWith($"data.{type}.")))
            {
                var newKey = key.Replace($"data.{type}.", string.Empty);
                var value = ConfigurationManager.AppSettings[key];
                Console.WriteLine("-> {" + newKey + "} = " + value);

                ScenarioContext.Current.Add(newKey, value);
            }
        }

    }
}