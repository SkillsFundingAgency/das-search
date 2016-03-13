namespace Sfa.Das.ApplicationServices.Models
{
    using System.Collections.Generic;

    using Eds.Das.Core.Domain.Model;

    public sealed class ProviderSearchResultsItem
    {
        public long Id { get; set; }

        public string UkPrn { get; set; }

        public string ProviderName { get; set; }

        public string VenueName { get; set; }

        public string PostCode { get; set; }

        public Coordinate Coordinate { get; set; }

        public int Radius { get; set; }

        public double Distance { get; set; }

        public int Standardscode { get; set; }

        public string Website { get; set; }

        public string TrainingLocationName { get; set; }

        public string TrainingLocationAddress { get; set; }

        public double EmployerSatisfaction { get; set; }

        public double LearnerSatisfaction { get; set; }
    }
}