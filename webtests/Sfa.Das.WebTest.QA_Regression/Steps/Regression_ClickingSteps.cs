using System;
using TechTalk.SpecFlow;

namespace Sfa.Das.WebTest.QA_Regression
{
    [Binding]
    public class Regression_ClickingSteps
    {
        [When(@"I choose (.*)")]
        public void WhenIChoose(string p0)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
