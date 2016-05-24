using System;
using TechTalk.SpecFlow;

namespace Sfa.Das.WebTest.QA_Regression.Steps
{
    [Binding]
    public class EnterRegressionDataSteps
    {
        [When(@"I enter (.*)")]
        public void WhenIEnter(string p0, Table table)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
