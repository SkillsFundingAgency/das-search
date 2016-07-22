using System.Collections.Generic;

namespace Sfa.Das.Sas.Indexer.Core.Services
{
    using Sfa.Das.Sas.Indexer.Core.Models;

    public interface IAchievmentRatesProvider

    {
        IEnumerable<AchievmentRateProvider> GetAllByProvider();

        IEnumerable<AchievmentRateNational> GetAllNational();
    }
}
