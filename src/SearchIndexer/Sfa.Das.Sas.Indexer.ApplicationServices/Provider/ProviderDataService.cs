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

        private readonly IAchievementRatesProvider _achievementRatesProvider;

        private readonly ILog _logger;

        public ProviderDataService(
            IProviderFeatures features,
            IGetApprenticeshipProviders providerRepository,
            IGetActiveProviders activeProviderClient,
            IMetaDataHelper metaDataHelper,
            IAchievementRatesProvider achievementRatesProvider,
            ILog logger)
        {
            _features = features;
            _providerRepository = providerRepository;
            _activeProviderClient = activeProviderClient;
            _metaDataHelper = metaDataHelper;
            _achievementRatesProvider = achievementRatesProvider;
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
            var byProvier = _achievementRatesProvider.GetAllByProvider();
            var national = _achievementRatesProvider.GetAllNational();

            await Task.WhenAll(frameworks, standards, providers);

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

        private static double? GetNationalOverallAchievementRate(List<AchievementRateNational> nationalAchievementRate)
        {
            var nationalOverallAchievementRate = nationalAchievementRate
                .OrderByDescending(m => m.HybridEndYear)
                .FirstOrDefault()?
                .OverallAchievementRate;

            if (nationalOverallAchievementRate != null)
            {
                return Math.Round((double)nationalOverallAchievementRate);
            }

            return null;
        }

        private static bool IsLevelFourOrHigher(string achievementRateProviderLevel, int level)
        {
            return achievementRateProviderLevel == "4+" && level > 3;
        }

        private static bool IsLevelTwoOrThree(string achievementRateProviderLevel, int level)
        {
            return (achievementRateProviderLevel == "2" || achievementRateProviderLevel == "3") && achievementRateProviderLevel == level.ToString();
        }

        private void UpdateStandard(StandardInformation si, List<StandardMetaData> standards, IEnumerable<AchievementRateProvider> achievementRates, IEnumerable<AchievementRateNational> nationalAchievementRates)
        {
            var metaData = standards.Find(m => m.Id == si.Code);

            if (metaData != null)
            {
                var achievementRate = achievementRates.Where(m =>
                    IsEqual(m.Ssa2Code, metaData.SectorSubjectAreaTier2))
                    .Where(m => TestLevel(m.ApprenticeshipLevel, metaData.NotionalEndLevel))
                    .ToList();

                var nationalAchievementRate = nationalAchievementRates.Where(m =>
                    IsEqual(m.Ssa2Code, metaData.SectorSubjectAreaTier2))
                    .Where(m => TestLevel(m.ApprenticeshipLevel, metaData.NotionalEndLevel))
                    .ToList();

                var rate = ExtractValues(achievementRate);
                si.OverallAchievementRate = rate.Item1;
                si.OverallCohort = rate.Item2;

                si.NationalOverallAchievementRate =
                    GetNationalOverallAchievementRate(nationalAchievementRate);
            }
        }

        private void UpdateFramework(FrameworkInformation fi, List<FrameworkMetaData> frameworks, IEnumerable<AchievementRateProvider> achievementRates, IEnumerable<AchievementRateNational> nationalAchievementRates)
        {
            var metaData = frameworks.Find(m =>
                m.FworkCode == fi.Code &&
                m.PwayCode == fi.PathwayCode &&
                m.ProgType == fi.ProgType);

            if (metaData != null)
            {
                var achievementRate = achievementRates.Where(m =>
                    IsEqual(m.Ssa2Code, metaData.SectorSubjectAreaTier2))
                    .Where(m => TestLevel(m.ApprenticeshipLevel, ApprenticeshipLevelMapper.MapToLevel(metaData.ProgType)))
                    .ToList();

                var nationalAchievementRate = nationalAchievementRates.Where(m =>
                    IsEqual(m.Ssa2Code, metaData.SectorSubjectAreaTier2))
                    .Where(m => TestLevel(m.ApprenticeshipLevel, ApprenticeshipLevelMapper.MapToLevel(metaData.ProgType)))
                    .ToList();

                var rate = ExtractValues(achievementRate);

                fi.OverallAchievementRate = rate.Item1;

                fi.OverallCohort = rate.Item2;

                fi.NationalOverallAchievementRate =
                    GetNationalOverallAchievementRate(nationalAchievementRate);
            }
        }

        private Tuple<double?, string> ExtractValues(List<AchievementRateProvider> achievementRate)
        {
            if (achievementRate.Count > 1)
            {
                _logger.Warn($"Multiple achievement rates found - UPPRN: {achievementRate.FirstOrDefault()?.Ukprn}");
            }

            double? v1;

            if (achievementRate.FirstOrDefault()?.OverallAchievementRate != null)
            {
                v1 = Math.Round((double)achievementRate.FirstOrDefault()?.OverallAchievementRate);
            }
            else
            {
                v1 = null;
            }

            var v2 = achievementRate.FirstOrDefault()?.OverallCohort;
            return new Tuple<double?, string>(v1, v2);
        }

        private bool IsEqual(double d1, double d2)
        {
            return Math.Abs(d1 - d2) < 0.01;
        }

        private bool TestLevel(string achievementRateProviderLevel, int level)
        {
            return IsLevelTwoOrThree(achievementRateProviderLevel, level) || IsLevelFourOrHigher(achievementRateProviderLevel, level);
        }
    }
}
