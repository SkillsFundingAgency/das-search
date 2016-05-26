using System;
using TechTalk.SpecFlow;

namespace Sfa.Das.WebTest.QA_Regression.Steps
{
    [Binding]
    public class VerifyElements_RegressionSteps
    {
        [Then(@"I see (.*) page")]
        public void ThenISeePage(string p0)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"I see the Text in table")]
        public void ThenISeeTheTextInTable(Table table)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
