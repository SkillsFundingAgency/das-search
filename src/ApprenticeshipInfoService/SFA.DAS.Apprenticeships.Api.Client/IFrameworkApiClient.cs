using System;
using System.Collections.Generic;
using SFA.DAS.Apprenticeships.Api.Types;

namespace SFA.DAS.Apprenticeships.Api.Client
{
    public interface IFrameworkApiClient : IDisposable
    {
        /// <summary>
        /// Get a single framework details
        /// GET /frameworks/{framework-id}
        /// </summary>
        /// <param name="frameworkId">an integer for the composite id {frameworkId}{pathway}{progType}</param>
        /// <returns>a framework details based on pathway and level</returns>
        Framework Get(string frameworkId);

        /// <summary>
        /// Get a single framework details
        /// GET /frameworks/{frameworkCode}{pathwayCode}{progamType}
        /// </summary>
        /// <param name="frameworkCode">an integer for the framework code</param>
        /// <param name="pathwayCode">an integer for the pathway code</param>
        /// <param name="progamType">an integer for the program type</param>
        /// <returns>a framework details based on pathway and level</returns>
        Framework Get(int frameworkCode, int pathwayCode, int progamType);

        /// <summary>
        /// Get a collection of frameworks
        /// GET /frameworks
        /// </summary>
        /// <returns>a collection of framework summaries</returns>
        IEnumerable<FrameworkSummary> FindAll();

        /// <summary>
        /// Check if a framework exists
        /// HEAD /frameworks/{framework-id}
        /// </summary>
        /// <param name="frameworkId">an integer for the composite id {frameworkId}{pathway}{progType}</param>
        /// <returns>bool</returns>
        bool Exists(string frameworkId);

        /// <summary>
        /// Check if a framework exists
        /// HEAD /frameworks/{frameworkCode}{pathwayCode}{progamType}
        /// </summary>
        /// <param name="frameworkCode">an integer for the framework code</param>
        /// <param name="pathwayCode">an integer for the pathway code</param>
        /// <param name="progamType">an integer for the program type</param>
        /// <returns>a framework details based on pathway and level</returns>
        bool Exists(int frameworkCode, int pathwayCode, int progamType);
    }
}