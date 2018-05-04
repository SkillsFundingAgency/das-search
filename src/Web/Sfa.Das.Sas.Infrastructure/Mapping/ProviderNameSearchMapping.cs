using System;
using System.Collections.Generic;
using System.Linq;
using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.Infrastructure.Mapping
{
    public class ProviderNameSearchMapping : IProviderNameSearchMapping
    {

        public IEnumerable<ProviderNameSearchResult> FilterNonMatchingAliases(string searchTerm, IEnumerable<ProviderNameSearchResult> resultsToFilter)
        {
            var resultsToReturn = new List<ProviderNameSearchResult>();
            foreach (var item in resultsToFilter)
            {
                var details = new ProviderNameSearchResult
                {
                    ProviderName = item.ProviderName,
                    UkPrn = item.UkPrn,
                    Aliases = item.Aliases?
                        .Where(m => m.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                        .Select(s => s.Trim()).ToList()
                };

                resultsToReturn.Add(details);
            }

            return resultsToReturn;
        }
    }
}