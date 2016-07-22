using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using LARSMetaDataToolBox.Models;
using LARSMetaDataToolBox.Serialization;
using LARSMetaDataToolBox.Settings;

namespace LARSMetaDataToolBox.Filters
{
    public class QualificationFilter
    {
        public static ICollection<FrameworkQualification> GetQualification(MetaDataBag bag, AppSettings appSettings)
        {
            var frameworkQualifications = new List<FrameworkQualification>();

            if (!File.Exists(appSettings.QualificationsFilePath))
            {
                bag.Frameworks = FilterFrameworks(bag.Frameworks);

                bag.LearningDeliveries =
                    bag.LearningDeliveries.Where(x => !x.EffectiveTo.HasValue || x.EffectiveTo.Value > DateTime.Now)
                        .ToList();

                bag.Fundings =
                    bag.Fundings.Where(x => !x.EffectiveTo.HasValue || x.EffectiveTo.Value > DateTime.Now)
                        .ToList();

                var timer = Stopwatch.StartNew();

                var count = 0;

                var total = bag.Frameworks.Count;

                foreach (var framework in bag.Frameworks)
                {
                    Console.WriteLine($"Getting qualification for framework {++count} / {total}");

                    var frameworkAims = bag.FrameworkAims.Where(x => x.FworkCode.Equals(framework.FworkCode) &&
                                                                     x.ProgType.Equals(framework.ProgType) &&
                                                                     x.PwayCode.Equals(framework.PwayCode)).ToList();
                    var qualifications =
                        (from aim in frameworkAims
                            join comp in bag.FrameworkComponentTypes on aim.FrameworkComponentType equals
                                comp.FrameworkComponentType
                            join ld in bag.LearningDeliveries on aim.LearnAimRef equals ld.LearnAimRef
                            select new FrameworkQualification
                            {
                                LearnAimRef = aim.LearnAimRef,
                                ProgType = aim.ProgType,
                                Title = ld.LearnAimRefTitle.Replace("(QCF)", string.Empty).Trim(),
                                CompetenceType = comp.FrameworkComponentType,
                                CompetenceDescription = comp.FrameworkComponentTypeDesc
                            }).ToList();

                    frameworkQualifications.AddRange(qualifications);
                }
                
                Console.WriteLine($"Time taken to find qualifications: {timer.Elapsed.ToString("g")}");

                FileSerializer.SerializeCollectionToFile(appSettings.QualificationsFilePath, frameworkQualifications);

            }
            else
            {
                frameworkQualifications =
                    FileSerializer.DeserializeCollectionFromFile<FrameworkQualification>(
                        appSettings.QualificationsFilePath).ToList();
            }

            return frameworkQualifications;
        }

        private static ICollection<FrameworkMetaData> FilterFrameworks(IEnumerable<FrameworkMetaData> frameworks)
        {
            var progTypeList = new[] { 2, 3, 20, 21, 22, 23 };

            return frameworks.Where(s => s.FworkCode > 399)
                .Where(s => s.PwayCode > 0)
                .Where(s => !s.EffectiveFrom.Equals(DateTime.MinValue))
                .Where(s => !s.EffectiveTo.HasValue || s.EffectiveTo > DateTime.Now)
                .Where(s => progTypeList.Contains(s.ProgType))
                .ToList();
        }

        //public static ICollection<FrameworkQualification> Filter(MetaDataBag bag)
        //{
        //        //var filteredOutQualification = qualifications.Where(q =>
        //        //    !q.FundingEffectiveTo.HasValue &&
        //        //    (!q.FundingRateWeight.HasValue || q.FundingRateWeight.Value == 0)).ToList();

        //        //filteredValues.AddRange(filteredOutQualification.Select(x => x.Title));
        //    }

        //    Console.WriteLine($"Time taken to find qualifications: {timer.Elapsed.ToString("g")}");
        //    //return filteredValues.Distinct();

        //    return results;
        //}
    }
}