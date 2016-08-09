using System.Collections.Generic;
using Sfa.Das.Sas.Core.Configuration;
using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.Web.ViewModels
{
    using Sfa.Das.Sas.ApplicationServices.Models;

    public class CookieViewModel
    {
        public string ImprovementUrl { get; set; }

        public string GoogleUrl { get; set; }

        public string ApplicationInsightsUrl { get; set; }

        public string AboutUrl { get; set; }

        public string SurveyProviderUrl { get; set; }
    }
}