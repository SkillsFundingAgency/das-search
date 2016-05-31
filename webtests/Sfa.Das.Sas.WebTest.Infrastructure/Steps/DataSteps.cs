namespace Sfa.Das.Sas.WebTest.Infrastructure.Steps
{
    using System;
    using System.Configuration;
    using System.Linq;

    using Sfa.Das.Sas.WebTest.Infrastructure.Services;

    using SpecBind.Helpers;

    using TechTalk.SpecFlow;

    [Binding]
    public class DataSteps
    {
        private readonly ITokenManager _tokenManager;

        private readonly ILog _log;

        public DataSteps(ITokenManager tokenManager, ILog log)
        {
            _tokenManager = tokenManager;
            _log = log;
        }

        [Given(@"I have data in the config")]
        public void GivenIHaveDataInTheConfig(Table data)
        {
            foreach (var row in data.Rows)
            {
                try
                {
                    var value = ConfigurationManager.AppSettings[row["Key"]];
                    this._tokenManager.SetToken(row["Token"], value);
                }
                catch (Exception ex)
                {
                    _log.Error("problem loading config", ex);
                }
            }
        }

    }
}