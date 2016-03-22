using Sfa.Eds.Das.Core.Configuration;

namespace Sfa.Eds.Das.Infrastructure.ElasticSearch
{
    using System;
    using System.Linq;

    using Core.Domain.Model;
    using Sfa.Das.ApplicationServices.Models;
    using Sfa.Eds.Das.Core.Domain.Services;
    using Sfa.Eds.Das.Core.Logging;

    public sealed class StandardRepository : IStandardRepository
    {
        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;
        private readonly ILog _applicationLogger;
        private readonly IConfigurationSettings _applicationSettings;

        public StandardRepository(IElasticsearchClientFactory elasticsearchClientFactory, ILog applicationLogger, IConfigurationSettings applicationSettings)
        {
            _elasticsearchClientFactory = elasticsearchClientFactory;
            _applicationLogger = applicationLogger;
            _applicationSettings = applicationSettings;
        }

        public Standard GetById(int id)
        {
            var client = this._elasticsearchClientFactory.Create();
            var results =
                client.Search<StandardSearchResultsItem>(s => s
                    .Index(_applicationSettings.StandardIndexesAlias)
                    .Types("standarddocument")
                    .From(0)
                    .Size(1)
                    .Query(q =>
                        q.QueryString(qs =>
                            qs.OnFields(e => e.StandardId)
                            .Query(id.ToString()))));

            if (results.ConnectionStatus.HttpStatusCode != 200)
            {
                _applicationLogger.Error($"Trying to get standard with id {id}");

                throw new ApplicationException($"Failed query standard with id {id}");
            }

            var document = results.Documents.Any() ? results.Documents.First() : null;

            if (document != null)
            {
                return new Standard
                           {
                               StandardId = document.StandardId,
                               Title = document.Title,
                               StandardPdfUrl = document.StandardPdfUrl,
                               AssessmentPlanPdfUrl = document.AssessmentPlanPdfUrl,
                               NotionalEndLevel = document.NotionalEndLevel,
                               JobRoles = document.JobRoles,
                               Keywords = document.Keywords,
                               TypicalLength = document.TypicalLength,
                               IntroductoryText = document.IntroductoryText,
                               EntryRequirements = document.EntryRequirements,
                               WhatApprenticesWillLearn = document.WhatApprenticesWillLearn,
                               Qualifications = document.Qualifications,
                               ProfessionalRegistration = document.ProfessionalRegistration,
                               OverviewOfRole = document.OverviewOfRole
                };
            }

            return null;
        }
    }
}
