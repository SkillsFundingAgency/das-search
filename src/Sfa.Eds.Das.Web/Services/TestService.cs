namespace Sfa.Eds.Das.Web.Services
{
    public class TestService : ITestService
    {
        public string Hello(string world)
        {
            return string.Format("Hello {0}", world);
        }
    }
}