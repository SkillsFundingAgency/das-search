using System.Collections.Generic;
using Sfa.Das.Sas.ApplicationServices.Logging;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Settings;
using Sfa.Das.Sas.Core.Domain.Model;
using SFA.DAS.NLog.Logger;

namespace Sfa.Das.Sas.ApplicationServices
{
    public sealed class ApprenticeshipSearchService : IApprenticeshipSearchService
    {
        private readonly IApprenticeshipSearchProvider _searchProvider;
        private readonly ILog _logger;
        private readonly IPaginationSettings _paginationSettings;

        public ApprenticeshipSearchService(
            IApprenticeshipSearchProvider searchProvider,
            ILog logger,
            IPaginationSettings paginationSettings)
        {
            _searchProvider = searchProvider;
            _logger = logger;
            _paginationSettings = paginationSettings;
        }

        public ApprenticeshipSearchResults SearchByKeyword(string keywords, int page, int take, int order, List<int> selectedLevels)
        {
            var takeElements = take == 0 ? _paginationSettings.DefaultResultsAmount : take;

            var results = new ApprenticeshipSearchResults();

            //MFCSTUB
            if (keywords == "software")
            {
                results = new ApprenticeshipSearchResults
                {
                    TotalResults = 2,
                    ResultsToTake = 2,
                    ActualPage = 1,
                    LastPage = 1,
                    SearchTerm = "software",
                    SortOrder = "ascending",
                    Results = new List<ApprenticeshipSearchResultsItem>
                    {
                        new ApprenticeshipSearchResultsItem
                        {
                            Title = "abc title",
                            StandardId = "standardId1",
                            JobRoles = new List<string> {"job 1", "job 2"},
                            Keywords = new List<string> {"software" },
                            FrameworkId = "frameworkId1",
                            FrameworkName = "framework name 1",
                            PathwayName = "pathway for f1",
                            Level = 1,
                            JobRoleItems = new List<JobRoleItem> {new JobRoleItem { Title="job 1 1", Description = "job 1 1 Desc"} },
                            Published = true,
                            Duration = 20,
                            TitleKeyword = "software"
                        },
                        new ApprenticeshipSearchResultsItem
                        {
                            Title = "def title",
                            StandardId = "standardId2",
                            JobRoles = new List<string> {"job 3", "job 4"},
                            Keywords = new List<string> {"software" },
                            FrameworkId = "frameworkId2",
                            FrameworkName = "framework name 2",
                            PathwayName = "pathway for f2",
                            Level = 1,
                            JobRoleItems = new List<JobRoleItem> {new JobRoleItem { Title="job 2 1", Description = "job 2 1 Desc"} },
                            Published = true,
                            Duration = 30,
                            TitleKeyword = "software"
                        }
                    },
                    HasError = false,
                    LevelAggregation = new Dictionary<int, long?>(),
                    SelectedLevels = new List<int> {1}
                };
            }
            else
            {
                results = _searchProvider.SearchByKeyword(keywords, page, takeElements, order, selectedLevels);
            }

            _logger.Info(
                "Apprenticeship Keyword Search",
                new ApprenticeshipSearchLogEntry { TotalHits = results?.TotalResults ?? -1, Keywords = keywords?.Split(' ') ?? new[] { "[empty]" } });

            return results;
        }
    }
}