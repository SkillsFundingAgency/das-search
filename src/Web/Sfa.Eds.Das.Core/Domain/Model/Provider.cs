namespace Sfa.Eds.Das.Core.Domain.Model
{
    using System.Collections.Generic;

    using Sfa.Das.ApplicationServices.Models;

    public sealed class Provider
    {
        public int Id { get; set; }

        public string UkPrn { get; set; }

        public string ProviderName { get; set; }

        public string VenueName { get; set; }

        public string PostCode { get; set; }

        public Coordinate Coordinate { get; set; }

        public int Radius { get; set; }

        public Address Address { get; set; }

        public double Distance { get; set; }

        public List<int> StandardsId { get; set; }

        public string Website { get; set; }

        public string TrainingLocationName { get; set; }

        public string TrainingLocationAddress { get; set; }

        public double EmployerSatisfaction { get; set; }

        public double LearnerSatisfaction { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public List<string> DeliveryModes { get; set; }

        public string StandardInfoUrl { get; set; }
    }
}
