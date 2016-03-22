namespace Sfa.Das.ApplicationServices.Models
{
    using System.Collections.Generic;
    using System.Runtime.Remoting.Activation;

    using Sfa.Eds.Das.Core.Domain.Model;

    public class ApprenticeshipSearchResultsItem
    {
        public string Title { get; set; }

        public int NotionalEndLevel { get; set; }

        // Standards
        public int StandardId { get; set; }

        public List<string> JobRoles { get; set; }

        public List<string> Keywords { get; set; }

        public TypicalLength TypicalLength { get; set; }

        // Frameworks
        public int FrameworkId { get; set; }

        public string FrameworkName { get; set; }

        public string PathwayName { get; set; }

        public string Level { get; set; }


    }
}