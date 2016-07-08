using System;
using System.Collections.Generic;
using MediatR;

namespace Sfa.Das.Sas.ApplicationServices.Queries
{
    using Sfa.Das.Sas.ApplicationServices.Models;

    public class ProviderSearchQuery
    {
        private string _postcode;

        public string PostCode
        {
            get { return _postcode; }
            set { _postcode = value?.Trim(); }
        }

        public int ApprenticeshipId { get; set; }

        public int Page { get; set; }

        public int Take { get; set; }

        public bool ShowAll { get; set; }

        public IEnumerable<string> DeliveryModes { get; set; }

        public string Keywords { get; set; }
    }
}
