namespace Sfa.Das.Sas.Indexer.Core.Models
{
    public class AchievementRateProvider
    {
        public long Ukprn { get; set; }

        public string Age { get; set; }

        public string ApprenticeshipLevel { get; set; }

        public string OverallCohort { get; set; }

        public double? OverallAchievementRate { get; set; }

        public string SectorSubjectAreaTier2 { get; set; }

        public double Ssa2Code { get; set; }
    }
}