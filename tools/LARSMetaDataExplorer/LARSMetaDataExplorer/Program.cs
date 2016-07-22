using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AngleSharp.Dom;
using LARSMetaDataToolBox.Factories;
using LARSMetaDataToolBox.Filters;
using LARSMetaDataToolBox.Models;
using LARSMetaDataToolBox.Settings;

namespace LARSMetaDataToolBox
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Getting all qualifications...");
            var completeTitleList = GetQualifications();

            Console.WriteLine("Outputting qualification titles to file...");
            File.WriteAllLines("All Qualifications.txt", completeTitleList);


            Console.WriteLine("Getting filtered qualifications...");
            var titles = GetFilteredQualifications();

            Console.WriteLine("Outputting filtered qualification titles to file...");
            File.WriteAllLines("Filtered Qualifications.txt", titles);

            Console.WriteLine("Reading outputted qualifications from file...");
            var filteredQualifications = File.ReadAllLines("Filtered Qualifications.txt");

            Console.WriteLine("Reading expected filtered qualifications from file...");
            var expectedQualifications = File.ReadAllLines("ExpectedFilteredQualifications.txt");

            Console.WriteLine("Filtering outputted qualifications...");
            expectedQualifications = expectedQualifications.Select(x => x.Replace("(QCF)", string.Empty)).ToArray();

            expectedQualifications = expectedQualifications.Distinct().ToArray();

            filteredQualifications = filteredQualifications.Select(x => x.Trim().ToLower()).ToArray();

            expectedQualifications = expectedQualifications.Select(x => x.Trim().ToLower()).ToArray();

            Console.WriteLine("Finding matches and missing qualifications from expected values...");
            var matches = filteredQualifications.Intersect(expectedQualifications).ToList();

            var newFilters = filteredQualifications.Except(expectedQualifications).ToList();

            var unseenTitles = expectedQualifications.Except(filteredQualifications).ToList();

            Console.WriteLine("Writing match results to file...");
            File.WriteAllLines("Additional Qualifications.txt", newFilters);
            File.WriteAllLines("Unseen Titles.txt", unseenTitles);
            File.WriteAllLines("Matches.txt", matches);

            Console.WriteLine("Finished processing, Press a key to quit.");
            Console.Read();
        }

        private static IEnumerable<string> GetQualifications()
        {
            var metaDataServiceFactory = new MetaDataServiceFactory(new LarsDataStreamFactory(), new CsvServiceFactory());

            var appSettings = new AppSettings();

            using (var metaDataService = metaDataServiceFactory.CreateService())
            {
                Console.WriteLine("Get metadata bag...");
                var bag = metaDataService.GetMetaDataBag(appSettings);

                Console.WriteLine("Getting qualifications...");
                var qualifications = QualificationFilter.GetQualification(bag, appSettings);

                return qualifications.Select(x => x.Title);
            }
        }



        private static IEnumerable<string> GetFilteredQualifications()
        {
            Console.WriteLine("Loading metadata service...");

            var metaDataServiceFactory = new MetaDataServiceFactory(new LarsDataStreamFactory(), new CsvServiceFactory());

            var appSettings = new AppSettings();

            using (var metaDataService = metaDataServiceFactory.CreateService())
            {
                Console.WriteLine("Get metadata bag...");
                var bag = metaDataService.GetMetaDataBag(appSettings);

                Console.WriteLine("Getting qualifications...");
                var qualifications = QualificationFilter.GetQualification(bag, appSettings);



                Console.WriteLine("Adding funding to qualifications...");
                //var connection = (from q in qualifications
                //    join f in bag.Fundings on q.LearnAimRef equals f.LearnAimRef
                //    select new
                //    {
                //        Q = q,
                //        F = f
                //    });

                //connection.ToList().ForEach(x =>
                //{
                //    x.Q.FundingRateWeight = x.F.RateWeighted;
                //    x.Q.FundingEffectiveTo = x.F.EffectiveTo;
                //});

                //qualifications = connection.Select(x => x.Q).ToList();

                foreach (var qualification in qualifications)
                {
                    var qualificationFundings =
                        bag.Fundings.Where(
                            x => x.LearnAimRef.Equals(qualification.LearnAimRef, StringComparison.OrdinalIgnoreCase)).ToList();

                    var funding = (qualificationFundings.FirstOrDefault(x => x.EffectiveTo == null && x.RateWeighted < 1) ??
                                   qualificationFundings.FirstOrDefault(x => x.EffectiveTo == null)) ??
                                   qualificationFundings.OrderByDescending(x => x.EffectiveTo).FirstOrDefault();

                    if (funding == null) continue;

                    qualification.FundingRateWeight = funding.RateWeighted;
                    qualification.FundingEffectiveTo = funding.EffectiveTo;
                }

                Console.WriteLine("Saving qualifications with funding details to file...");

                var dataItems = qualifications.Select(x => new
                {
                    LearnRef = x.LearnAimRef,
                    Title = x.Title,
                    RateWeight = x.FundingRateWeight,
                    DateTo = x.FundingEffectiveTo?.ToString("d") ?? "N/A",
                    ProgType = x.ProgType
                });

                var dataOutput = dataItems.Select(x => $"{x.LearnRef}, {x.Title}, {x.RateWeight}, {x.DateTo}, {x.ProgType}");
              
                File.WriteAllLines("DataSheet.csv", dataOutput);

                Console.WriteLine("filtering qualification on funding values...");
                qualifications =
                    qualifications.Where(
                        x => !x.FundingEffectiveTo.HasValue || x.FundingEffectiveTo.Value > DateTime.Today).ToList();

                qualifications =
                    qualifications.Where(x => x.FundingRateWeight < 1).ToList();

                qualifications = qualifications.Where(x => x.ProgType <= 3).ToList();

                Console.WriteLine("Returning filtered qualification...");
                return qualifications.Select(x => x.Title).Distinct().OrderBy(x => x).ToList();
            }
        }
    }
}