namespace SFA.DAS.Apprenticeships.Api.Types
{
    public class StandardSummary
    {
        /// <summary>
        /// The standard identifier from LARS
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// a link to the standard details
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// The standard title
        /// </summary>
        public string Title { get; set; }

        public TypicalLength TypicalLength { get; set; }

        public int Level { get; set; }
    }
}
