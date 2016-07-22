using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Das.Sas.Indexer.ApplicationServices.Provider
{
    using Sfa.Das.Sas.Indexer.ApplicationServices.Services;
    using Sfa.Das.Sas.Indexer.ApplicationServices.Standard;
    using Sfa.Das.Sas.Indexer.Core.Extensions;
    using Sfa.Das.Sas.Indexer.Core.Logging;
    using Sfa.Das.Sas.Indexer.Core.Models;
    using Sfa.Das.Sas.Indexer.Core.Models.Framework;
    using Sfa.Das.Sas.Indexer.Core.Models.Provider;
    using Sfa.Das.Sas.Indexer.Core.Services;

    public class ProviderDataService : IProviderDataService
    {
        private readonly IProviderFeatures _features;

        private readonly IGetApprenticeshipProviders _providerRepository;

        private readonly IGetActiveProviders _activeProviderClient;

        private readonly IMetaDataHelper _metaDataHelper;

        private readonly IAchievmentRatesProvider _achievmentRatesProvider;

        private readonly ILog _logger;

        public ProviderDataService(
            IProviderFeatures features,
            IGetApprenticeshipProviders providerRepository,
            IGetActiveProviders activeProviderClient,
            IMetaDataHelper metaDataHelper,
            IAchievmentRatesProvider achievmentRatesProvider,
            ILog logger)
        {
            _features = features;
            _providerRepository = providerRepository;
            _activeProviderClient = activeProviderClient;
            _metaDataHelper = metaDataHelper;
            _achievmentRatesProvider = achievmentRatesProvider;
            _logger = logger;
        }

        public async Task<ICollection<Provider>> GetProviders()
        {
            // From Course directory
            var providers = Task.Run(() => _providerRepository.GetApprenticeshipProvidersAsync());

            // From LARS
            var frameworks = Task.Run(() => _metaDataHelper.GetAllFrameworkMetaData());
            var standards = Task.Run(() => _metaDataHelper.GetAllStandardsMetaData());

            // From database
            var byProvier = _achievmentRatesProvider.GetAllByProvider();
            var national = _achievmentRatesProvider.GetAllNational();

            Task.WaitAll(frameworks, standards, providers);

            var ps = providers.Result.ToArray();
            foreach (var provider in ps)
            {
                var byProvidersFiltered = byProvier.Where(bp => bp.Ukprn == provider.Ukprn);
                provider.Frameworks.ForEach(m => UpdateFramework(m, frameworks.Result, byProvidersFiltered, national));
                provider.Standards.ForEach(m => UpdateStandard(m, standards.Result, byProvidersFiltered, national));
            }

            if (_features.FilterInactiveProviders)
            {
                var activeProviders = _activeProviderClient.GetActiveProviders().ToList();

                return ps.Where(x => activeProviders.Contains(x.Ukprn)).ToList();
            }

            return ps;
        }

        private void UpdateStandard(StandardInformation si, List<StandardMetaData> standards, IEnumerable<AchievmentRateProvider> achievmentRates, IEnumerable<AchievmentRateNational> nationalAchievmentRates)
        {
            var metaData = standards.Find(m => m.Id == si.Code);

            if (metaData != null)
            {
                var achievmentRate = achievmentRates.Where(m =>
                    IsEqual(m.Ssa2Code, metaData.SectorSubjectAreaTier2))
                    .Where(m => TestLevel(m.ApprenticeshipLevel, metaData.NotionalEndLevel))
                    .ToList();

                var nationalAchievmentRate = nationalAchievmentRates.Where(m =>
                    IsEqual(m.SSA2Code, metaData.SectorSubjectAreaTier2))
                    .Where(m => TestLevel(m.ApprenticeshipLevel, metaData.NotionalEndLevel))
                    .ToList();

                var rate = ExtractValues(achievmentRate);
                si.OverallAchievementRate = rate.Item1;
                si.OverallCohort = rate.Item2;

                si.NationalOverallAchievementRate =
                    nationalAchievmentRate
                    .OrderByDescending(m => m.HybridEndYear)
                    .FirstOrDefault()
                    ?.OverallAchievementRate;
            }
        }

        private void UpdateFramework(FrameworkInformation fi, List<FrameworkMetaData> frameworks, IEnumerable<AchievmentRateProvider> achievmentRates, IEnumerable<AchievmentRateNational> nationalAchievmentRates)
        {
            var metaData = frameworks.Find(m =>
                m.FworkCode == fi.Code &&
                m.PwayCode == fi.PathwayCode &&
                m.ProgType == fi.ProgType);

            if (metaData != null)
            {
                var achievementRate = achievmentRates.Where(m =>
                    IsEqual(m.Ssa2Code, metaData.SectorSubjectAreaTier2))
                    .Where(m => TestLevel(m.ApprenticeshipLevel, ApprenticeshipLevelMapper.MapLevel(metaData.ProgType)))
                    .ToList();

                var nationalAchievmentRate = nationalAchievmentRates.Where(m =>
                    IsEqual(m.SSA2Code, metaData.SectorSubjectAreaTier2))
                    .Where(m => TestLevel(m.ApprenticeshipLevel, ApprenticeshipLevelMapper.MapLevel(metaData.ProgType)))
                    .ToList();

                var rate = ExtractValues(achievementRate);
                fi.OverallAchievementRate = rate.Item1;
                fi.OverallCohort = rate.Item2;

                fi.NationalOverallAchievementRate = nationalAchievmentRate.FirstOrDefault()?.OverallAchievementRate;
            }
        }

        private Tuple<double, string> ExtractValues(List<AchievmentRateProvider> achievementRate)
        {
            if (achievementRate.Count > 1)
            {
                _logger.Warn($"Multiple achievement rates found - UPPRN: {achievementRate.FirstOrDefault()?.Ukprn}");
            }

            var v1 = achievementRate.FirstOrDefault()?.OverallAchievementRate ?? 0;
            var v2 = achievementRate.FirstOrDefault()?.OverallCohort;
            return new Tuple<double, string>(v1, v2);
        }

        private bool IsEqual(double d1, double d2)
        {
            return Math.Abs(d1 - d2) < 0.01;
        }

        private bool TestLevel(string achievmentRateProviderLevel, int level)
        {
            return ((achievmentRateProviderLevel == "2" || achievmentRateProviderLevel == "3") && achievmentRateProviderLevel == level.ToString())
                   || level > 3;
        }
    }
}
