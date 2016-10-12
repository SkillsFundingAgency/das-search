namespace SFA.DAS.Apprenticeships.Api.Types
{
    public class Provider
    {
        public int Ukprn { get; set; }

        public string ProviderName { get; set; }

        public string Uri { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public bool NationalProvider { get; set; }

        public string Website { get; set; }

        public double EmployerSatisfaction { get; set; }

        public double LearnerSatisfaction { get; set; }
    }
}
