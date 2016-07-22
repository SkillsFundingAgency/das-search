namespace Sfa.Das.Sas.Indexer.Infrastructure.DapperBD
{
    using System.Collections.Generic;

    using Sfa.Das.Sas.Indexer.Core.Models;
    using Sfa.Das.Sas.Indexer.Core.Services;

    public class AchievementRatesProvider : IAchievementRatesProvider
    {
        private readonly IDatabaseProvider _databaseProvider;

        public AchievementRatesProvider(IDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;
        }

        public IEnumerable<AchievementRateProvider> GetAllByProvider()
        {
            var query = @"SELECT 
                    [UKPRN], 
                    [Age],
                    [Apprenticeship Level] as ApprenticeshipLevel,
                    [Overall Cohort] as OverallCohort, 
                    [Overall Achivement Rate %] as OverallAchievementRate,
                    [Sector Subject Area Tier 2] as SectorSubjectAreaTier2,
                    [SSA2 Code] as SSA2Code
                    FROM ar_by_provider
                    WHERE [Age] = 'All Age'
                    AND [Sector Subject Area Tier 2] <> 'All SSA T2'
                    AND [Apprenticeship Level] <> 'All'
                    ";
            return _databaseProvider.Query<AchievementRateProvider>(query);
        }

        public IEnumerable<AchievementRateNational> GetAllNational()
        {
            var query = @"
                    SELECT 
                        [Institution Type] as InstitutionType,
                        [Hybrid End Year] as HybridEndYear,
                        [Age],
                        [Sector Subject Area Tier 1] as SectorSubjectAreaTier1,
                        [Sector Subject Area Tier 2] as SectorSubjectAreaTier2,
                        [Apprenticeship Level] as ApprenticeshipLevel,
                        [Overall Achievement Rate %] as OverallAchievementRate,
                        [SSA2] as SSA2Code
                    FROM ar_national
                    WHERE [Institution Type] = 'All Institution Type'
                    AND [Age] = 'All Age'
                    AND [Sector Subject Area Tier 2] <> 'All SSA T2'
                    AND [Apprenticeship Level] <> 'All'
                    ";
            return _databaseProvider.Query<AchievementRateNational>(query);
        }
    }
}