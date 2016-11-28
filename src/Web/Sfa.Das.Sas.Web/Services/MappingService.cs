using System;
using System.Linq;
using AutoMapper;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Core.Logging;
using Sfa.Das.Sas.Web.Services.MappingActions;
using Sfa.Das.Sas.Web.Services.MappingActions.Helpers;
using Sfa.Das.Sas.Web.Services.MappingActions.ValueResolvers;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Services
{
    public class MappingService : IMappingService
    {
        private readonly ILog _logger;
        private readonly IMapper _mapper;

        public MappingService(ILog logger)
        {
            _logger = logger;
            Configuration = Config();
            _mapper = Configuration.CreateMapper();
        }

        public MapperConfiguration Configuration { get; private set; }

        public TDest Map<TSource, TDest>(TSource source)
        {
            try
            {
                return _mapper.Map<TSource, TDest>(source);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error mapping objects");
                throw;
            }
        }

        public TDest Map<TSource, TDest>(TSource source, Action<IMappingOperationOptions<TSource, TDest>> opts)
        {
            try
            {
                return _mapper.Map(source, opts);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error mapping objects");
                throw;
            }
        }

        private static void CreateProviderDetailsMappings(IMapperConfiguration cfg)
        {
            cfg.CreateMap<StatsResponse, StatsViewModel>()
                   .ForMember(dest => dest.StandardCount, opt => opt.MapFrom(source => source.StandardCount))
                   .ForMember(dest => dest.FrameworkCount, opt => opt.MapFrom(source => source.FrameworkCount))
                   .ForMember(dest => dest.ProviderCount, opt => opt.MapFrom(source => source.ProviderCount))
                   .ForMember(dest => dest.StandardOffer, opt => opt.MapFrom(source => source.StandardOffer))
                   .ForMember(dest => dest.FrameworkOffer, opt => opt.MapFrom(source => source.FrameworkOffer))
                   .ForMember(dest => dest.ExpiringFrameworks30, opt => opt.MapFrom(source => source.ExpiringFrameworks30))
                   .ForMember(dest => dest.ExpiringFrameworks90, opt => opt.MapFrom(source => source.ExpiringFrameworks90))
                   .ForMember(dest => dest.StandardsWithProviders, opt => opt.MapFrom(source => source.StandardsWithProviders))
                   .ForMember(dest => dest.StandardsWithoutProviders, opt => opt.MapFrom(source => source.StandardsWithoutProviders))
                   .ForMember(dest => dest.FrameworksWithProviders, opt => opt.MapFrom(source => source.FrameworksWithProviders))
                   .ForMember(dest => dest.FrameworksWithoutProviders, opt => opt.MapFrom(source => source.FrameworksWithoutProviders))
                   ;

            cfg.CreateMap<DetailProviderResponse, ApprenticeshipDetailsViewModel>()
                   .ForMember(dest => dest.Address, opt => opt.MapFrom(source => source.ApprenticeshipDetails.Location.Address))
                   .ForMember(dest => dest.Apprenticeship, opt => opt.MapFrom(source => source.ApprenticeshipDetails.Product.Apprenticeship))
                   .ForMember(dest => dest.ContactInformation, opt => opt.MapFrom(source => source.ApprenticeshipDetails.Provider.ContactInformation))
                   .ForMember(dest => dest.DeliveryModes, opt => opt.MapFrom(source => source.ApprenticeshipDetails.Product.DeliveryModes))
                   .ForMember(dest => dest.Location, opt => opt.MapFrom(source => source.ApprenticeshipDetails.Location))
                   .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.ApprenticeshipDetails.Provider.Name))
                   .ForMember(dest => dest.NationalProvider, opt => opt.MapFrom(source => source.ApprenticeshipDetails.Provider.NationalProvider))
                   .ForMember(dest => dest.Ukprn, opt => opt.MapFrom(source => source.ApprenticeshipDetails.Provider.UkPrn))
                   .ForMember(dest => dest.IsHigherEducationInstitute, opt => opt.MapFrom(source => source.ApprenticeshipDetails.Provider.IsHigherEducationInstitute))
                   .ForMember(dest => dest.EmployerSatisfaction, opt => opt.MapFrom(source => source.ApprenticeshipDetails.Product.EmployerSatisfaction))
                   .ForMember(dest => dest.LearnerSatisfaction, opt => opt.MapFrom(source => source.ApprenticeshipDetails.Product.LearnerSatisfaction))
                   .ForMember(dest => dest.ProviderMarketingInfo, opt => opt.MapFrom(source => source.ApprenticeshipDetails.Product.ProviderMarketingInfo))
                   .ForMember(x => x.EmployerSatisfactionMessage, y => y.MapFrom(z => ProviderMappingHelper.GetPercentageText(z.ApprenticeshipDetails.Product.EmployerSatisfaction, z.ApprenticeshipDetails.Provider.IsHigherEducationInstitute)))
                   .ForMember(x => x.LearnerSatisfactionMessage, y => y.MapFrom(z => ProviderMappingHelper.GetPercentageText(z.ApprenticeshipDetails.Product.LearnerSatisfaction, z.ApprenticeshipDetails.Provider.IsHigherEducationInstitute)))
                   .ForMember(dest => dest.SurveyUrl, y => y.Ignore())
                   .ForMember(dest => dest.SatisfactionSourceUrl, y => y.Ignore())
                   .ForMember(dest => dest.AchievementRateSourceUrl, y => y.Ignore())
                   .ForMember(x => x.AchievementRateMessage, y => y.MapFrom(z => ProviderMappingHelper.GetPercentageText(z.ApprenticeshipDetails.Product.AchievementRate)))
                   .ForMember(x => x.AchievementRate, y => y.MapFrom(z => z.ApprenticeshipDetails.Product.AchievementRate))
                   .ForMember(x => x.NationalAchievementRateMessage, y => y.MapFrom(z => ProviderMappingHelper.GetPercentageText(z.ApprenticeshipDetails.Product.NationalAchievementRate)))
                   .ForMember(x => x.NationalAchievementRate, y => y.MapFrom(z => z.ApprenticeshipDetails.Product.NationalAchievementRate))
                   .ForMember(x => x.OverallCohort, y => y.ResolveUsing<OverallCohortResolver>().FromMember(z => z.ApprenticeshipDetails.Product.OverallCohort))
                   .ForMember(x => x.ApprenticeshipName, y => y.MapFrom(z => ApprenticeshipMappingHelper.FrameworkTitle(z.ApprenticeshipName)))
                   .ForMember(x => x.LocationAddressLine, y => y.MapFrom(z =>
                        ProviderMappingHelper.GetCommaList(z.ApprenticeshipDetails.Location.LocationName, z.ApprenticeshipDetails.Location.Address.Address1, z.ApprenticeshipDetails.Location.Address.Address2, z.ApprenticeshipDetails.Location.Address.Town, z.ApprenticeshipDetails.Location.Address.County, z.ApprenticeshipDetails.Location.Address.Postcode)))
                   ;

            cfg.CreateMap<IApprenticeshipProviderSearchResultsItem, StandardProviderResultItemViewModel>()
                .ForMember(x => x.EmployerSatisfactionMessage, y => y.MapFrom(z => ProviderMappingHelper.GetPercentageText(z.EmployerSatisfaction, z.IsHigherEducationInstitute)))
                .ForMember(x => x.LearnerSatisfactionMessage, y => y.MapFrom(z => ProviderMappingHelper.GetPercentageText(z.LearnerSatisfaction, z.IsHigherEducationInstitute)))
                .ForMember(x => x.LocationAddressLine, y => y.Ignore())
                .ForMember(x => x.NationalProvider, y => y.Ignore())
                .ForMember(x => x.AchievementRateMessage, y => y.Ignore())
                .ForMember(x => x.StandardCode, y => y.Ignore())
                .ForMember(x => x.DeliveryOptionsMessage, y => y.Ignore())
                .ForMember(x => x.LocationId, y => y.Ignore())
                .ForMember(x => x.LocationName, y => y.Ignore())
                .ForMember(x => x.Address, y => y.Ignore())
                ;

            // ToDo: Do we need to specify if mapping is defined in concrete class?
            cfg.CreateMap<IApprenticeshipProviderSearchResultsItem, FrameworkProviderResultItemViewModel>()
                .ForMember(x => x.EmployerSatisfactionMessage, y => y.MapFrom(z => ProviderMappingHelper.GetPercentageText(z.EmployerSatisfaction, z.IsHigherEducationInstitute)))
                .ForMember(x => x.LearnerSatisfactionMessage, y => y.MapFrom(z => ProviderMappingHelper.GetPercentageText(z.LearnerSatisfaction, z.IsHigherEducationInstitute)))
                .ForMember(x => x.LocationAddressLine, y => y.Ignore())
                .ForMember(x => x.NationalProvider, y => y.Ignore())
                .ForMember(x => x.AchievementRateMessage, y => y.Ignore())
                .ForMember(x => x.PathwayCode, y => y.Ignore())
                .ForMember(x => x.Level, y => y.Ignore())
                .ForMember(x => x.FrameworkId, y => y.Ignore())
                .ForMember(x => x.FrameworkCode, y => y.Ignore())
                .ForMember(x => x.DeliveryOptionsMessage, y => y.Ignore())
                .ForMember(x => x.LocationId, y => y.Ignore())
                .ForMember(x => x.LocationName, y => y.Ignore())
                .ForMember(x => x.Address, y => y.Ignore())
                ;

            cfg.CreateMap<StandardProviderSearchResultsItem, StandardProviderResultItemViewModel>()
                .ForMember(x => x.StandardCode, y => y.MapFrom(z => z.StandardCode))
                .ForMember(x => x.EmployerSatisfactionMessage, y => y.MapFrom(z => ProviderMappingHelper.GetPercentageText(z.EmployerSatisfaction, z.IsHigherEducationInstitute)))
                .ForMember(x => x.LearnerSatisfactionMessage, y => y.MapFrom(z => ProviderMappingHelper.GetPercentageText(z.LearnerSatisfaction, z.IsHigherEducationInstitute)))
                .ForMember(x => x.AchievementRateMessage, y => y.MapFrom(z => ProviderMappingHelper.GetPercentageText(z.OverallAchievementRate)))
                .ForMember(x => x.LocationAddressLine, y => y.MapFrom(z => ProviderMappingHelper.GetLocationAddressLine(z.TrainingLocations.Single(x => x.LocationId == z.MatchingLocationId))))
                .ForMember(x => x.DeliveryOptionsMessage, y => y.ResolveUsing<DeliveryOptionResolver>().FromMember(z => z.DeliveryModes))
                .ForMember(x => x.LocationId, y => y.MapFrom(z => z.MatchingLocationId))
                .ForMember(x => x.LocationName, y => y.MapFrom(z => z.TrainingLocations.Single(x => x.LocationId == z.MatchingLocationId).LocationName))
                .ForMember(x => x.Address, y => y.MapFrom(z => z.TrainingLocations.Single(x => x.LocationId == z.MatchingLocationId).Address))
                ;

            cfg.CreateMap<FrameworkProviderSearchResultsItem, FrameworkProviderResultItemViewModel>()
                .ForMember(x => x.FrameworkId, y => y.MapFrom(z => z.FrameworkId))
                .ForMember(x => x.EmployerSatisfactionMessage, y => y.MapFrom(z => ProviderMappingHelper.GetPercentageText(z.EmployerSatisfaction, z.IsHigherEducationInstitute)))
                .ForMember(x => x.LearnerSatisfactionMessage, y => y.MapFrom(z => ProviderMappingHelper.GetPercentageText(z.LearnerSatisfaction, z.IsHigherEducationInstitute)))
                .ForMember(x => x.AchievementRateMessage, y => y.MapFrom(z => ProviderMappingHelper.GetPercentageText(z.OverallAchievementRate)))
                .ForMember(x => x.LocationAddressLine, y => y.MapFrom(z => ProviderMappingHelper.GetLocationAddressLine(z.TrainingLocations.Single(x => x.LocationId == z.MatchingLocationId))))
                .ForMember(x => x.DeliveryOptionsMessage, y => y.ResolveUsing<DeliveryOptionResolver>().FromMember(z => z.DeliveryModes))
                .ForMember(x => x.LocationId, y => y.MapFrom(z => z.MatchingLocationId))
                .ForMember(x => x.LocationName, y => y.MapFrom(z => z.TrainingLocations.Single(x => x.LocationId == z.MatchingLocationId).LocationName))
                .ForMember(x => x.Address, y => y.MapFrom(z => z.TrainingLocations.Single(x => x.LocationId == z.MatchingLocationId).Address))
                ;

            // Provider detail page
            cfg.CreateMap<ApprenticeshipDetails, ApprenticeshipDetailsViewModel>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(source => source.Location.Address))
            .ForMember(dest => dest.Apprenticeship, opt => opt.MapFrom(source => source.Product.Apprenticeship))
            .ForMember(dest => dest.Apprenticeship, opt => opt.MapFrom(source => source.Product.Apprenticeship))
            .ForMember(dest => dest.ContactInformation, opt => opt.MapFrom(source => source.Provider.ContactInformation))
            .ForMember(dest => dest.DeliveryModes, opt => opt.MapFrom(source => source.Product.DeliveryModes))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(source => source.Location))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.Provider.Name))
            .ForMember(dest => dest.NationalProvider, opt => opt.MapFrom(source => source.Provider.NationalProvider))
            .ForMember(dest => dest.Ukprn, opt => opt.MapFrom(source => source.Provider.UkPrn))
            .ForMember(dest => dest.IsHigherEducationInstitute, opt => opt.MapFrom(source => source.Provider.IsHigherEducationInstitute))
            .ForMember(dest => dest.EmployerSatisfaction, opt => opt.MapFrom(source => source.Product.EmployerSatisfaction))
            .ForMember(dest => dest.LearnerSatisfaction, opt => opt.MapFrom(source => source.Product.LearnerSatisfaction))
            .ForMember(dest => dest.ProviderMarketingInfo, opt => opt.MapFrom(source => source.Product.ProviderMarketingInfo))
            .ForMember(x => x.EmployerSatisfactionMessage, y => y.MapFrom(z => ProviderMappingHelper.GetPercentageText(z.Product.EmployerSatisfaction, z.Provider.IsHigherEducationInstitute)))
            .ForMember(x => x.LearnerSatisfactionMessage, y => y.MapFrom(z => ProviderMappingHelper.GetPercentageText(z.Product.LearnerSatisfaction, z.Provider.IsHigherEducationInstitute)))
            .ForMember(x => x.ApprenticeshipName, y => y.Ignore())
            .ForMember(x => x.ApprenticeshipLevel, y => y.Ignore())
            .ForMember(x => x.ApprenticeshipType, y => y.Ignore())
            .ForMember(x => x.SatisfactionSourceUrl, y => y.Ignore())
            .ForMember(x => x.AchievementRateSourceUrl, y => y.Ignore())
            .ForMember(x => x.SurveyUrl, y => y.Ignore())
            .ForMember(x => x.AchievementRateMessage, y => y.MapFrom(z => ProviderMappingHelper.GetPercentageText(z.Product.AchievementRate)))
            .ForMember(x => x.AchievementRate, y => y.MapFrom(z => z.Product.AchievementRate))
            .ForMember(x => x.NationalAchievementRateMessage, y => y.MapFrom(z => ProviderMappingHelper.GetPercentageText(z.Product.NationalAchievementRate)))
            .ForMember(x => x.NationalAchievementRate, y => y.MapFrom(z => z.Product.NationalAchievementRate))
            .ForMember(x => x.OverallCohort, y => y.ResolveUsing<OverallCohortResolver>().FromMember(z => z.Product.OverallCohort))
            .ForMember(x => x.LocationAddressLine, y => y.MapFrom(z =>
                ProviderMappingHelper.GetCommaList(z.Location.LocationName, z.Location.Address.Address1, z.Location.Address.Address2, z.Location.Address.Town, z.Location.Address.County, z.Location.Address.Postcode)))
            .AfterMap<ProviderViewModelMappingAction>();
        }

        private static void CreateApprenticeshipSearchResultsMappings(IMapperConfiguration cfg)
        {
            // Apprenticeship search listing  -> mix of standard and framework
            cfg.CreateMap<ApprenticeshipSearchResponse, ApprenticeshipSearchResultViewModel>()
                .ForMember(x => x.AggregationLevel, opt => opt.ResolveUsing<AggregationLevelValueResolver>())
                .ForMember(x => x.LastPage, y => y.MapFrom(z => SearchMappingHelper.CalculateLastPage(z.TotalResults, z.ResultsToTake)));

            // Nexzt
            cfg.CreateMap<ApprenticeshipSearchResultsItem, ApprenticeshipSearchResultItemViewModel>()
                .ForMember(x => x.TypicalLengthMessage, y => y.Ignore()) // set in aftermap
                .ForMember(x => x.ApprenticeshipType, y => y.Ignore()) // set in aftermap
                .AfterMap<ApprenticeshipSearchResultItemViewModelMappingAction>();
        }

        private static void CreateApprenticeshipDetailsMappings(IMapperConfiguration cfg)
        {
            // Standard detail page
            cfg.CreateMap<GetStandardResponse, StandardViewModel>()
                .ForMember(x => x.StandardId, y => y.MapFrom(z => z.Standard.StandardId))
                .ForMember(x => x.AssessmentPlanPdf, y => y.MapFrom(z => z.Standard.AssessmentPlanPdf))
                .ForMember(x => x.EntryRequirements, y => y.MapFrom(z => z.Standard.EntryRequirements))
                .ForMember(x => x.Level, y => y.MapFrom(z => z.Standard.Level))
                .ForMember(x => x.OverviewOfRole, y => y.MapFrom(z => z.Standard.OverviewOfRole))
                .ForMember(x => x.ProfessionalRegistration, y => y.MapFrom(z => z.Standard.ProfessionalRegistration))
                .ForMember(x => x.Qualifications, y => y.MapFrom(z => z.Standard.Qualifications))
                .ForMember(x => x.SearchTerm, y => y.MapFrom(z => z.SearchTerms))
                .ForMember(x => x.StandardPdf, y => y.MapFrom(z => z.Standard.StandardPdf))
                .ForMember(x => x.Title, y => y.MapFrom(z => z.Standard.Title))
                .ForMember(x => x.TypicalLengthMessage, y => y.MapFrom(z => ApprenticeshipMappingHelper.GetTypicalLengthMessage(z.Standard.TypicalLength)))
                .ForMember(x => x.WhatApprenticesWillLearn, y => y.MapFrom(z => z.Standard.WhatApprenticesWillLearn))
                ;

            // Framework detail page
            cfg.CreateMap<GetFrameworkResponse, FrameworkViewModel>()
                .ForMember(x => x.CombinedQualificiation, y => y.MapFrom(z => z.Framework.CombinedQualification))
                .ForMember(x => x.CompetencyQualification, y => y.MapFrom(z => z.Framework.CompetencyQualification))
                .ForMember(x => x.CompletionQualifications, y => y.MapFrom(z => z.Framework.CompletionQualifications))
                .ForMember(x => x.EntryRequirements, y => y.MapFrom(z => z.Framework.EntryRequirements))
                .ForMember(x => x.ExpiryDateString, y => y.MapFrom(z => ApprenticeshipMappingHelper.GetExpirydateAsString(z.Framework.ExpiryDate)))
                .ForMember(x => x.FrameworkId, y => y.MapFrom(z => z.Framework.FrameworkId))
                .ForMember(x => x.FrameworkOverview, y => y.MapFrom(z => z.Framework.FrameworkOverview))
                .ForMember(x => x.JobRoles, y => y.MapFrom(z => ApprenticeshipMappingHelper.GetTitlesFromJobRoles(z.Framework.JobRoleItems)))
                .ForMember(x => x.KnowledgeQualification, y => y.MapFrom(z => z.Framework.KnowledgeQualification))
                .ForMember(x => x.Level, y => y.MapFrom(z => z.Framework.Level))
                .ForMember(x => x.ProfessionalRegistration, y => y.ResolveUsing<FrameworkInformationResolver>().FromMember(z => z.Framework.ProfessionalRegistration))
                .ForMember(x => x.SearchTerm, y => y.MapFrom(z => z.SearchTerms))
                .ForMember(x => x.Title, y => y.MapFrom(z => ApprenticeshipMappingHelper.FrameworkTitle(z.Framework.Title)))
                .ForMember(x => x.TypicalLengthMessage, y => y.MapFrom(z => ApprenticeshipMappingHelper.GetTypicalLengthMessage(z.Framework.TypicalLength)))
                ;
        }

        private static void CreateProviderSearchMappings(IMapperConfiguration cfg)
        {
            cfg.CreateMap<GetFrameworkProvidersResponse, ProviderSearchViewModel>()
                .ForMember(x => x.Title, y => y.ResolveUsing<FrameworkTitleWithLevelResolver>())
                .ForMember(x => x.ApprenticeshipId, y => y.MapFrom(z => z.FrameworkId))
                .ForMember(x => x.SearchTerms, y => y.MapFrom(z => z.Keywords))
                .ForMember(x => x.HasError, y => y.MapFrom(z => z.HasErrors))
                .ForMember(x => x.PostUrl, y => y.Ignore())
                .ForMember(x => x.WrongPostcode, y => y.Ignore());

            cfg.CreateMap<GetStandardProvidersResponse, ProviderSearchViewModel>()
                .ForMember(x => x.ApprenticeshipId, y => y.MapFrom(z => z.StandardId))
                .ForMember(x => x.SearchTerms, y => y.MapFrom(z => z.Keywords))
                .ForMember(x => x.HasError, y => y.MapFrom(z => z.HasErrors))
                .ForMember(x => x.PostUrl, y => y.Ignore())
                .ForMember(x => x.WrongPostcode, y => y.Ignore());

            // Provider search
            cfg.CreateMap<StandardProviderSearchResponse, ProviderStandardSearchResultViewModel>()
                .ForMember(x => x.AbsolutePath, y => y.Ignore())
                .ForMember(x => x.ActualPage, y => y.MapFrom(z => z.CurrentPage))
                .ForMember(x => x.Hits, y => y.MapFrom(z => z.Results.Hits))
                .ForMember(x => x.PostCode, y => y.MapFrom(z => z.Results.PostCode))
                .ForMember(x => x.PostCodeMissing, y => y.MapFrom(z => z.Results.PostCodeMissing))
                .ForMember(x => x.ResultsToTake, y => y.MapFrom(z => z.Results.ResultsToTake))
                .ForMember(x => x.ShowAll, y => y.MapFrom(z => z.ShowAllProviders))
                .ForMember(x => x.StandardId, y => y.MapFrom(z => z.Results.StandardId))
                .ForMember(x => x.StandardName, y => y.MapFrom(z => z.Results.StandardName))
                .ForMember(x => x.StandardLevel, y => y.MapFrom(z => z.Results.StandardLevel))
                .ForMember(x => x.TotalResults, y => y.MapFrom(z => z.Results.TotalResults))
                .ForMember(x => x.ShowNationalProviders, y => y.MapFrom(z => z.ShowOnlyNationalProviders))
                .ForMember(x => x.HasError, y => y.MapFrom(z => !z.Success))
                .ForMember(x => x.DeliveryModes, opt => opt.ResolveUsing<DeliveryModesValueResolver>().FromMember(z => z.Results))
                .ForMember(x => x.NationalProviders, opt => opt.ResolveUsing<NationalProvidersValueResolver>().FromMember(z => z.Results))
                .ForMember(x => x.LastPage, opt => opt.ResolveUsing<LastPageValueResolver>().FromMember(z => z.Results));

            // ToDo: CF ->  Rename models?
            cfg.CreateMap<FrameworkProviderSearchResponse, ProviderFrameworkSearchResultViewModel>()
                .ForMember(x => x.AbsolutePath, y => y.Ignore())
                .ForMember(x => x.ActualPage, y => y.MapFrom(z => z.CurrentPage))
                .ForMember(x => x.Hits, y => y.MapFrom(z => z.Results.Hits))
                .ForMember(x => x.PostCode, y => y.MapFrom(z => z.Results.PostCode))
                .ForMember(x => x.PostCodeMissing, y => y.MapFrom(z => z.Results.PostCodeMissing))
                .ForMember(x => x.ResultsToTake, y => y.MapFrom(z => z.Results.ResultsToTake))
                .ForMember(x => x.Title, y => y.MapFrom(z => ApprenticeshipMappingHelper.FrameworkTitle(z.Results.Title)))
                .ForMember(x => x.FrameworkLevel, y => y.MapFrom(z => z.Results.FrameworkLevel))
                .ForMember(x => x.ShowAll, y => y.MapFrom(z => z.ShowAllProviders))
                .ForMember(x => x.FrameworkCode, y => y.MapFrom(z => z.Results.FrameworkCode))
                .ForMember(x => x.FrameworkId, y => y.MapFrom(z => z.Results.FrameworkId))
                .ForMember(x => x.FrameworkName, y => y.MapFrom(z => z.Results.FrameworkName))
                .ForMember(x => x.PathwayName, y => y.MapFrom(z => z.Results.PathwayName))
                .ForMember(x => x.TotalResults, y => y.MapFrom(z => z.Results.TotalResults))
                .ForMember(x => x.HasError, y => y.MapFrom(z => !z.Success))
                .ForMember(x => x.ShowNationalProviders, y => y.MapFrom(z => z.ShowOnlyNationalProviders))
                .ForMember(x => x.TotalProvidersCountry, y => y.MapFrom(z => z.TotalResultsForCountry))
                .ForMember(x => x.DeliveryModes, opt => opt.ResolveUsing<DeliveryModesValueResolver>().FromMember(z => z.Results))
                .ForMember(x => x.NationalProviders, opt => opt.ResolveUsing<NationalProvidersValueResolver>().FromMember(z => z.Results))
                .ForMember(x => x.LastPage, opt => opt.ResolveUsing<LastPageValueResolver>().FromMember(z => z.Results));
        }

        private static MapperConfiguration Config()
        {
            return new MapperConfiguration(cfg =>
            {
                CreateApprenticeshipSearchResultsMappings(cfg);

                CreateApprenticeshipDetailsMappings(cfg);

                CreateProviderSearchMappings(cfg);

                CreateProviderDetailsMappings(cfg);
            });
        }
    }
}