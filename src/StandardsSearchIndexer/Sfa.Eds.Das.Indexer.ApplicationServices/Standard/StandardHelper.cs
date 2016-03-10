namespace Sfa.Eds.Das.Indexer.ApplicationServices.Standard
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Services;
    using Nest;

    using Sfa.Eds.Das.Indexer.ApplicationServices.Infrastructure;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Models;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Services;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Services.Interfaces;
    using Sfa.Eds.Das.Indexer.Core;
    using Sfa.Eds.Das.Indexer.Core.Models;

    public class StandardHelper : IGenericIndexerHelper<MetaDataItem>
    {
        private readonly IBlobStorageHelper _blobStorageHelper;

        private readonly IElasticClient _client;

        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;

        private readonly IMetaDataHelper _metaDataHelper;

        private readonly IIndexMaintenanceService _indexMaintenanceService;

        private readonly IStandardIndexSettings _settings;

        private readonly ILog Log;

        public StandardHelper(IBlobStorageHelper blobStorageHelper,
            IStandardIndexSettings settings,
            IElasticsearchClientFactory elasticsearchClientFactory,
            IMetaDataHelper metaDataHelper,
            IIndexMaintenanceService indexMaintenanceService,
            ILog log)
        {
            _blobStorageHelper = blobStorageHelper;
            _settings = settings;
            _elasticsearchClientFactory = elasticsearchClientFactory;
            _metaDataHelper = metaDataHelper;
            _indexMaintenanceService = indexMaintenanceService;
            Log = log;

            _client = _elasticsearchClientFactory.GetElasticClient();
        }

        public async Task IndexEntries(DateTime scheduledRefreshDateTime, ICollection<MetaDataItem> entries)
        {

            try
            {

                Log.Debug("Indexing " + entries.Count() + " standards");

                var indexNameAndDateExtension = _indexMaintenanceService.GetIndexNameAndDateExtension(scheduledRefreshDateTime, _settings.StandardIndexesAlias);
                await IndexStandards(indexNameAndDateExtension, entries).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.Error("Error indexing PDFs", ex);
            }
        }

        public Task<ICollection<MetaDataItem>> LoadEntries()
        {
            UpdateMetadataRepositoryWithNewStandards();
            Log.Info("Indexing standard PDFs...");

            return Task.FromResult<ICollection<MetaDataItem>>(GetStandardsMetaDataFromGit().ToList());
        }

        public bool CreateIndex(DateTime scheduledRefreshDateTime)
        {
            var indexName = _indexMaintenanceService.GetIndexNameAndDateExtension(scheduledRefreshDateTime, _settings.StandardIndexesAlias);
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
            var indexName = _indexMaintenanceService.GetIndexNameAndDateExtension(scheduledRefreshDateTime, _settings.StandardIndexesAlias);

            return _client.Search<StandardDocument>(s => s.Index(indexName).From(0).Size(1000).MatchAll()).Documents.Any();
        }

        public void SwapIndexes(DateTime scheduledRefreshDateTime)
        {
            var indexAlias = _settings.StandardIndexesAlias;
            var newIndexName = _indexMaintenanceService.GetIndexNameAndDateExtension(scheduledRefreshDateTime, _settings.StandardIndexesAlias);

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
            var indicesToBeDelete = _indexMaintenanceService.GetOldIndices(_settings.StandardIndexesAlias, scheduledRefreshDateTime, _client.IndicesStats().Indices);

            Log.Debug($"Deleting {indicesToBeDelete.Count} old standard indexes...");
            foreach (var index in indicesToBeDelete)
            {
                Log.Debug($"Deleting {index}");
                _client.DeleteIndex(index);
            }
            Log.Debug("Deletion completed...");
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
                catch (Exception ex)
                {
                    Log.Error("Error indexing standard PDF", ex);
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
            catch (Exception ex)
            {
                Log.Error("Error creating document", ex);

                throw;
            }
        }
    }
}