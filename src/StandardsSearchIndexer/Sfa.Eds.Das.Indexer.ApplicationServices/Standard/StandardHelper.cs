namespace Sfa.Eds.Das.Indexer.ApplicationServices.Standard
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Nest;

    using Sfa.Eds.Das.Indexer.ApplicationServices.Infrastructure;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Models;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Services.Interfaces;
    using Sfa.Eds.Das.Indexer.Common.Extensions;
    using Sfa.Eds.Das.Indexer.Core;
    using Sfa.Eds.Das.Indexer.Core.Models;
    using Sfa.Eds.Das.StandardIndexer.Settings;

    public class StandardHelper : IGenericIndexerHelper<MetaDataItem>
    {
        private readonly IBlobStorageHelper _blobStorageHelper;

        private readonly IElasticClient _client;

        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;

        private readonly IGetStandardLevel _getStandardLevel;

        private readonly IMetaDataHelper _metaDataHelper;

        private readonly IStandardIndexSettings _settings;

        private readonly ILog Log;

        public StandardHelper(
            IGetStandardLevel getStandardLevel,
            IBlobStorageHelper blobStorageHelper,
            IStandardIndexSettings settings,
            IElasticsearchClientFactory elasticsearchClientFactory,
            IMetaDataHelper metaDataHelper,
            ILog log)
        {
            _getStandardLevel = getStandardLevel;
            _blobStorageHelper = blobStorageHelper;
            _settings = settings;
            _elasticsearchClientFactory = elasticsearchClientFactory;
            _metaDataHelper = metaDataHelper;
            Log = log;

            _client = _elasticsearchClientFactory.GetElasticClient();
        }

        public async Task IndexEntries(DateTime scheduledRefreshDateTime, ICollection<MetaDataItem> entries)
        {
            //Log.Debug("Uploading " + entries.Count() + " standard's PDF to Azure");

            try
            {
                //await entries.ForEachAsync(UploadStandardPdf).ConfigureAwait(false);

                Log.Debug("Indexing " + entries.Count() + " standards");

                var indexNameAndDateExtension = GetIndexNameAndDateExtension(scheduledRefreshDateTime);
                await IndexStandards(indexNameAndDateExtension, entries).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Log.Error("Error indexing PDFs", e);
            }
        }

        public ICollection<MetaDataItem> LoadEntries()
        {
            UpdateMetadataRepositoryWithNewStandards();
            Log.Info("Indexing standard PDFs...");
            return GetStandardsMetaDataFromGit().ToList();
        }

        public bool CreateIndex(DateTime scheduledRefreshDateTime)
        {
            var indexName = GetIndexNameAndDateExtension(scheduledRefreshDateTime);
            var indexExistsResponse = _client.IndexExists(indexName);

            // If it already exists and is empty, let's delete it.
            if (indexExistsResponse.Exists)
            {
                Log.Warn("Index already exists, deleting and creating a new one");

                _client.DeleteIndex(indexName);
            }

            // create index
            _client.CreateIndex(indexName, c => c.AddMapping<StandardDocument>(m => m.MapFromAttributes()));

            return _client.IndexExists(indexName).Exists;
        }

        public async Task IndexStandards(DateTime scheduledRefreshDateTime, IEnumerable<MetaDataItem> standards)
        {
            await IndexEntries(scheduledRefreshDateTime, standards.ToList());
        }

        public bool IsIndexCorrectlyCreated(DateTime scheduledRefreshDateTime)
        {
            var indexName = GetIndexNameAndDateExtension(scheduledRefreshDateTime);

            return _client.Search<StandardDocument>(s => s.Index(indexName).From(0).Size(1000).MatchAll()).Documents.Any();
        }

        public void SwapIndexes(DateTime scheduledRefreshDateTime)
        {
            var indexAlias = _settings.StandardIndexesAlias;
            var newIndexName = GetIndexNameAndDateExtension(scheduledRefreshDateTime);

            if (!CheckIfAliasExists(indexAlias))
            {
                Log.Warn("Alias doesn't exists, creating a new one...");

                CreateAlias(newIndexName);
            }

            var existingIndexesOnAlias = _client.GetIndicesPointingToAlias(indexAlias);
            var aliasRequest = new AliasRequest { Actions = new List<IAliasAction>() };

            foreach (var existingIndexOnAlias in existingIndexesOnAlias)
            {
                aliasRequest.Actions.Add(
                    new AliasRemoveAction { Remove = new AliasRemoveOperation { Alias = indexAlias, Index = existingIndexOnAlias } });
            }

            aliasRequest.Actions.Add(new AliasAddAction { Add = new AliasAddOperation { Alias = indexAlias, Index = newIndexName } });
            _client.Alias(aliasRequest);
        }

        public void DeleteOldIndexes(DateTime scheduledRefreshDateTime)
        {
            var indices = _client.IndicesStats().Indices;

            var standardIndexToday = string.Concat("cistandardindexesalias-", scheduledRefreshDateTime.ToUniversalTime().ToString("yyyy-MM-dd"));
            var standardIndexOneDayAgo = string.Concat("cistandardindexesalias-", scheduledRefreshDateTime.AddDays(-1).ToUniversalTime().ToString("yyyy-MM-dd"));
            var standardIndexTwoDaysAgo = string.Concat("cistandardindexesalias-", scheduledRefreshDateTime.AddDays(-2).ToUniversalTime().ToString("yyyy-MM-dd"));
            var providerIndex = "provider";
            var logIndex = "log";
            var kibanaIndex = "kibana";

            foreach (var index in indices.Where(index => !index.Key.Contains(standardIndexToday)
                                                         && !index.Key.Contains(standardIndexOneDayAgo)
                                                         && !index.Key.Contains(standardIndexTwoDaysAgo)
                                                         && !index.Key.Contains(providerIndex)
                                                         && !index.Key.Contains(logIndex)
                                                         && !index.Key.Contains(kibanaIndex)))
            {
                _client.DeleteIndex(index.Key);
            }
        }

        public string GetIndexNameAndDateExtension(DateTime dateTime)
        {
            return
                string.Format("{0}-{1}", _settings.StandardIndexesAlias, dateTime.ToUniversalTime().ToString("yyyy-MM-dd-HH"))
                    .ToLower(CultureInfo.InvariantCulture);
        }

        public void UpdateMetadataRepositoryWithNewStandards()
        {
            _metaDataHelper.UpdateMetadataRepository();
        }

        public IEnumerable<MetaDataItem> GetStandardsMetaDataFromGit()
        {
            return _metaDataHelper.GetAllStandardsMetaData();
        }

        private void CreateAlias(string indexName)
        {
            _client.Alias(a => a.Add(add => add.Index(indexName).Alias(_settings.StandardIndexesAlias)));
        }

        private bool CheckIfAliasExists(string aliasName)
        {
            var aliasExistsResponse = _client.AliasExists(aliasName);

            return aliasExistsResponse.Exists;
        }

        private async Task UploadStandardPdf(MetaDataItem standard)
        {
            if (!_blobStorageHelper.FileExists(_settings.StandardPdfContainer, string.Format(standard.Id.ToString(), ".pdf")))
            {
                await
                    _blobStorageHelper.UploadPdfFromUrl(
                        _settings.StandardPdfContainer,
                        string.Format(standard.Id.ToString(), ".pdf"),
                        standard.StandardPdfUrl).ConfigureAwait(false);
            }
        }

        private async Task IndexStandards(string indexName, IEnumerable<MetaDataItem> standards)
        {
            // index the items
            foreach (var standard in standards)
            {
                try
                {
                    var doc = await CreateDocument(standard);

                    _client.Index(doc, i => i.Index(indexName).Id(doc.StandardId));
                }
                catch (Exception e)
                {
                    Log.Error("Error indexing standard PDF", e);
                    throw;
                }
            }
        }

        private async Task<StandardDocument> CreateDocument(MetaDataItem standard)
        {
            try
            {
                var attachment = new Attachment
                                     {
                                         Content =
                                             Convert.ToBase64String(
                                                 await
                                                 _blobStorageHelper.ReadStandardPdfAsync(
                                                     _settings.StandardPdfContainer,
                                                     string.Format(standard.Id.ToString(), ".pdf"))),
                                         ContentType = _settings.StandardContentType,
                                         Name = standard.PdfFileName
                                     };

                var doc = new StandardDocument
                              {
                                  StandardId = standard.Id,
                                  Title = standard.Title,
                                  JobRoles = standard.JobRoles,
                                  NotionalEndLevel = standard.NotionalEndLevel,
                                  PdfFileName = standard.PdfFileName,
                                  StandardPdf = standard.StandardPdfUrl,
                                  AssessmentPlanPdf = standard.AssessmentPlanPdfUrl,
                                  TypicalLength = standard.TypicalLength,
                                  IntroductoryText = standard.IntroductoryText,
                                  OverviewOfRole = standard.OverviewOfRole,
                                  EntryRequirements = standard.EntryRequirements,
                                  WhatApprenticesWillLearn = standard.WhatApprenticesWillLearn,
                                  Qualifications = standard.Qualifications,
                                  ProfessionalRegistration = standard.ProfessionalRegistration,
                              };

                return doc;
            }
            catch (Exception e)
            {
                Log.Error("Error creating document", e);

                throw;
            }
        }
    }
}