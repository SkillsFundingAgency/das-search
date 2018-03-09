using System.Collections.Generic;
using System.Linq;
using Sfa.Das.Sas.ApplicationServices.Models;

namespace Sfa.Das.Sas.Infrastructure.Elasticsearch
{
    public sealed class DeduplicationService: IDeduplicationService
    {
        public IEnumerable<T> DedupeAtYourLocationOnlyDocuments<T>(IEnumerable<T> documents)
            where T : IApprenticeshipProviderSearchResultsItem
        {
            var dedupedDocuments = new List<T>();
            var ukprnsToIgnore = new List<int>();

            foreach (var document in documents)
            {
                if (document.DeliveryModes.Count == 1 && document.DeliveryModes[0] == "100PercentEmployer")
                {
                    if (ukprnsToIgnore.Count(x => x == document.Ukprn) == 0)
                    {
                        dedupedDocuments.Add(document);
                        ukprnsToIgnore.Add(document.Ukprn);
                    }
                }
                else
                {
                    dedupedDocuments.Add(document);
                }
            }

            return dedupedDocuments;
        }
    }
}