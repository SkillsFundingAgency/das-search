namespace Sfa.Das.Sas.WebTest.Infrastructure.Steps
{
    using System;
    using System.Configuration;
    using System.Linq;

    using SpecBind.Helpers;

    using TechTalk.SpecFlow;

    [Binding]
    public class DataSteps
    {
        private readonly ITokenManager _tokenManager;

        public DataSteps(ITokenManager tokenManager)
        {
            _tokenManager = tokenManager;
        }

        [Given(@"I have data for a (.*)")]
        public void GivenIHaveDataForA(string type)
        {
            foreach (var key in ConfigurationManager.AppSettings.AllKeys.Where(x => x.StartsWith($"data.{type}.")))
            {
                var newKey = key.Replace($"data.{type}.", string.Empty);
                var value = ConfigurationManager.AppSettings[key];
                Console.WriteLine("-> {" + newKey + "} = " + value);

                this._tokenManager.SetToken(newKey, value);
            }
        }

    }
}