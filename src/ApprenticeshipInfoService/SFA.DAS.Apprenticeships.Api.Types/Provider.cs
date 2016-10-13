namespace SFA.DAS.Apprenticeships.Api.Types
{
    public class Provider
    {
        /// <summary>
        /// UK provider reference number which is not unique
        /// </summary>
        public int Ukprn { get; set; }

        public string ProviderName { get; set; }

        /// <summary>
        /// Is this provider also an employer
        /// </summary>
        public bool IsEmployerProvider { get; set; }

        /// <summary>
        /// TODO Uri to the full provider information
        /// </summary>
        public string Uri { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public bool NationalProvider { get; set; }

        public string Website { get; set; }

        public double EmployerSatisfaction { get; set; }

        public double LearnerSatisfaction { get; set; }
    }
}
