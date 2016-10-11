using System;
using System.Collections.Generic;
using SFA.DAS.Apprenticeships.Api.Client.Models;

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
    }
}