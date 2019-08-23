using Sfa.Das.Sas.Core.Configuration;
using System;
using System.Web;

namespace Sfa.Das.Sas.Shared.Components.Configuration
{
    internal class SaveApprenticeshipUrlBuilder
    {
        private readonly IFatConfigurationSettings _config;

        public SaveApprenticeshipUrlBuilder(IFatConfigurationSettings config)
        {
            _config = config;
        }

        public Uri GenerateSaveUrl(string apprenticeshipId, int ukprn)
        {
            var saveUrl = new Uri(_config.SaveEmployerFavouritesUrl);
            var builder = new UriBuilder(saveUrl);
            var query = HttpUtility.ParseQueryString(builder.Query);

            query["apprenticeshipId"] = apprenticeshipId;
            query["ukprn"] = ukprn.ToString();

            builder.Query = query.ToString();

            return builder.Uri;
        }
    }

}
