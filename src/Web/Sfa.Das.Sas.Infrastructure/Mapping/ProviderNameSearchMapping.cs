using System;
using System.Collections.Generic;
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
                var details = new ProviderNameSearchResult { ProviderName = item.ProviderName, UkPrn = item.UkPrn };
                if (item.Aliases != null)
                {
                    var aliasesToKeep = new List<string>();
                    foreach (var alias in item.Aliases)
                    {
                        if (alias.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) <= -1)
                        { continue; }
                        var aliasToAdd = alias;
                        aliasesToKeep.Add(aliasToAdd);
                    }
                    details.Aliases = aliasesToKeep;
                }
                resultsToReturn.Add(details);
            }

            return resultsToReturn;
        }
    }
}
