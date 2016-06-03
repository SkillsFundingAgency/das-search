namespace Sfa.Das.WebTest.QA_Regression
{
    using TechTalk.SpecFlow;

    [Binding]
    public class OnPageRegressionSteps
    {
        [Given(@"I was on the (.*)")]
        [When(@"I am on the (.*) page")]
        [Then(@"I am on the (.*) page")]
        public void IamOnThePage(string page)
        {
            
        }
        

    }

}
