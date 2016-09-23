namespace Sfa.Das.ApprenticeshipInfoService.Api.Helpers
{
    using System.Linq;
    using System.Net.Http;

    public class ControllerHelper : IControllerHelper
    {
        public int GetPageFromUrl(HttpRequestMessage requestMessage)
        {
            var pageString = requestMessage.GetQueryNameValuePairs().LastOrDefault(x => x.Key == "page").Value;

            if (string.IsNullOrEmpty(pageString))
            {
                pageString = "1";
            }

            var result = int.Parse(pageString);

            return result > 0 ? result : 1;
        }
    }
}