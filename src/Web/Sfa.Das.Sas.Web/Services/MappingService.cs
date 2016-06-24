using System;
using AutoMapper;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Core.Logging;
using Sfa.Das.Sas.Web.Services.MappingActions;
using Sfa.Das.Sas.Web.Services.MappingActions.ValueResolvers;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Services
{
    using System.Linq;

    using Microsoft.Ajax.Utilities;

    using Sfa.Das.Sas.Web.Services.MappingActions.Helpers;

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
            TDest tempDest = default(TDest);
            try
            {
                tempDest = _mapper.Map<TSource, TDest>(source);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error mapping objects");
            }

            return tempDest;
        }

        public TDest Map<TSource, TDest>(TSource source, Action<IMappingOperationOptions<TSource, TDest>> opts)
        {
            TDest tempDest = default(TDest);
            try
            {
                tempDest = _mapper.Map(source, opts);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error mapping objects");
            }

            return tempDest;
        }

        private static void CreateProviderDetailsMappings(IMapperConfiguration cfg)
        {
            cfg.CreateMap<DetailProviderResponse, ApprenticeshipDetailsViewModel>()
                   .ForMember(dest => dest.Address, opt => opt.MapFrom(source => source.ApprenticeshipDetails.Location.Address))
                   .ForMember(dest => dest.Apprenticeship, opt => opt.MapFrom(source => source.ApprenticeshipDetails.Product.Apprenticeship))
                   .ForMember(dest => dest.ContactInformation, opt => opt.MapFrom(source => source.ApprenticeshipDetails.Provider.ContactInformation))
                   .ForMember(dest => dest.DeliveryModes, opt => opt.MapFrom(source => source.ApprenticeshipDetails.Product.DeliveryModes))
                   .ForMember(dest => dest.Location, opt => opt.MapFrom(source => source.ApprenticeshipDetails.Location))
                   .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.ApprenticeshipDetails.Provider.Name))
                   .ForMember(dest => dest.ProviderId, opt => opt.MapFrom(source => source.ApprenticeshipDetails.Provider.Id))
                   .ForMember(dest => dest.EmployerSatisfaction, opt => opt.MapFrom(source => source.ApprenticeshipDetails.Product.EmployerSatisfaction))
                   .ForMember(dest => dest.LearnerSatisfaction, opt => opt.MapFrom(source => source.ApprenticeshipDetails.Product.LearnerSatisfaction))
                   .ForMember(dest => dest.ProviderMarketingInfo, opt => opt.MapFrom(source => source.ApprenticeshipDetails.Product.ProviderMarketingInfo))
                   .ForMember(dest => dest.ProviderId, opt => opt.MapFrom(source => source.ApprenticeshipDetails.Provider.Id))
                   .ForMember(dest => dest.EmployerSatisfactionMessage, opt => opt.ResolveUsing<EmployerSatisfactionResolver>().FromMember(z => z.ApprenticeshipDetails.Product.EmployerSatisfaction))
                   .ForMember(dest => dest.LearnerSatisfactionMessage, opt => opt.ResolveUsing<EmployerSatisfactionResolver>().FromMember(z => z.ApprenticeshipDetails.Product.LearnerSatisfaction))
                   ;

            cfg.CreateMap<IApprenticeshipProviderSearchResultsItem, StandardProviderResultItemViewModel>()
                .ForMember(x => x.EmployerSatisfactionMessage, y => y.ResolveUsing<EmployerSatisfactionResolver>().FromMember(z => z.EmployerSatisfaction))
                .ForMember(x => x.LearnerSatisfactionMessage, y => y.ResolveUsing<EmployerSatisfactionResolver>().FromMember(z => z.LearnerSatisfaction))
                .ForMember(x => x.StandardCode, y => y.Ignore())
                .ForMember(x => x.IsShortlisted, y => y.Ignore()) // Done in aftemap
                ;

            cfg.CreateMap<IApprenticeshipProviderSearchResultsItem, FrameworkProviderResultItemViewModel>()
                .ForMember(x => x.EmployerSatisfactionMessage, y => y.ResolveUsing<EmployerSatisfactionResolver>().FromMember(z => z.EmployerSatisfaction))
                .ForMember(x => x.LearnerSatisfactionMessage, y => y.ResolveUsing<EmployerSatisfactionResolver>().FromMember(z => z.LearnerSatisfaction))
                .ForMember(x => x.PathwayCode, y => y.Ignore())
                .ForMember(x => x.Level, y => y.Ignore())
                .ForMember(x => x.FrameworkId, y => y.Ignore())
                .ForMember(x => x.FrameworkCode, y => y.Ignore())
                .ForMember(x => x.IsShortlisted, y => y.Ignore()) // Done in aftemap
                ;

            cfg.CreateMap<StandardProviderSearchResultsItem, StandardProviderResultItemViewModel>()
                .ForMember(x => x.StandardCode, y => y.MapFrom(z => z.StandardCode))
                .ForMember(x => x.IsShortlisted, y => y.Ignore())
                .ForMember(x => x.EmployerSatisfactionMessage, y => y.ResolveUsing<EmployerSatisfactionResolver>().FromMember(z => z.EmployerSatisfaction))
                .ForMember(x => x.LearnerSatisfactionMessage, y => y.ResolveUsing<EmployerSatisfactionResolver>().FromMember(z => z.LearnerSatisfaction))
                ;

            cfg.CreateMap<FrameworkProviderSearchResultsItem, FrameworkProviderResultItemViewModel>()
                .ForMember(x => x.FrameworkId, y => y.MapFrom(z => z.FrameworkId))
                .ForMember(x => x.IsShortlisted, y => y.Ignore())
                .ForMember(x => x.EmployerSatisfactionMessage, y => y.ResolveUsing<EmployerSatisfactionResolver>().FromMember(z => z.EmployerSatisfaction))
                .ForMember(x => x.LearnerSatisfactionMessage, y => y.ResolveUsing<EmployerSatisfactionResolver>().FromMember(z => z.LearnerSatisfaction))
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
            .ForMember(dest => dest.ProviderId, opt => opt.MapFrom(source => source.Provider.Id))
            .ForMember(dest => dest.EmployerSatisfaction, opt => opt.MapFrom(source => source.Product.EmployerSatisfaction))
            .ForMember(dest => dest.LearnerSatisfaction, opt => opt.MapFrom(source => source.Product.LearnerSatisfaction))
            .ForMember(dest => dest.ProviderMarketingInfo, opt => opt.MapFrom(source => source.Product.ProviderMarketingInfo))
            .ForMember(x => x.EmployerSatisfactionMessage, y => y.ResolveUsing<EmployerSatisfactionResolver>().FromMember(z => z.Product.EmployerSatisfaction))
            .ForMember(x => x.LearnerSatisfactionMessage, y => y.ResolveUsing<EmployerSatisfactionResolver>().FromMember(z => z.Product.LearnerSatisfaction))
            .ForMember(x => x.ApprenticeshipNameWithLevel, y => y.Ignore())
            .ForMember(x => x.ApprenticeshipLevel, y => y.Ignore())
            .ForMember(x => x.ApprenticeshipType, y => y.Ignore())
            .ForMember(x => x.IsShortlisted, y => y.Ignore())
            .AfterMap<ProviderViewModelMappingAction>();
        }

        private static void CreateApprenticeshipSearchResultsMappings(IMapperConfiguration cfg)
        {
            // Apprenticeship search listing  -> mix of standard and framework
            cfg.CreateMap<ApprenticeshipSearchResults, ApprenticeshipSearchResultViewModel>()
                .ForMember(x => x.AggregationLevel, opt => opt.ResolveUsing<AggregationLevelValueResolver>())
                .ForMember(x => x.ShortlistedFrameworks, y => y.Ignore()) // In controller
                .ForMember(x => x.ShortlistedStandards, y => y.Ignore()) // In controller
                .AfterMap((src, dest) => src.LastPage = dest.ResultsToTake != 0 ? (int)Math.Ceiling((double)src.TotalResults / src.ResultsToTake) : src.LastPage)
                ;

            // Nexzt
            cfg.CreateMap<ApprenticeshipSearchResultsItem, ApprenticeshipSearchResultItemViewModel>()
                .ForMember(x => x.TypicalLengthMessage, y => y.Ignore()) // set in aftermap
                .ForMember(x => x.ApprenticeshipType, y => y.Ignore()) // set in aftermap
                .AfterMap<ApprenticeshipSearchResultItemViewModelMappingAction>();
        }

        private static void CreateApprenticeshipDetailsMappings(IMapperConfiguration cfg)
        {
            // Standard detail page
            cfg.CreateMap<Standard, StandardViewModel>()
                .ForMember(x => x.IsShortlisted, y => y.Ignore()) // In controller
                .ForMember(x => x.SearchTerm, y => y.Ignore()) // In controller
                .ForMember(x => x.TypicalLengthMessage, y => y.MapFrom(z => ApprenticeshipMappingHelper.GetTypicalLengthMessage(z.TypicalLength)))
                ;

            // Framework detail page
            cfg.CreateMap<Framework, FrameworkViewModel>()
                .ForMember(x => x.ExpiryDateString, y => y.MapFrom(z => ApprenticeshipMappingHelper.GetExpirydateAsString(z.ExpiryDate)))
                .ForMember(x => x.IsShortlisted, y => y.Ignore()) // In controller
                .ForMember(x => x.SearchTerm, y => y.Ignore()) // In controller
                .ForMember(x => x.JobRoles, y => y.MapFrom(z => ApprenticeshipMappingHelper.GetTitlesFromJobRoles(z.JobRoleItems)))
                .ForMember(x => x.TypicalLengthMessage, y => y.MapFrom(z => ApprenticeshipMappingHelper.GetTypicalLengthMessage(z.TypicalLength)))
                ;
        }

        private static void CreateProviderSearchMappings(IMapperConfiguration cfg)
        {
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
                .ForMember(x => x.TotalResults, y => y.MapFrom(z => z.Results.TotalResults))
                .ForMember(x => x.HasError, y => y.MapFrom(z => !z.Success))
                .ForMember(x => x.DeliveryModes, opt => opt.ResolveUsing<DeliveryModesValueResolver>().FromMember(z => z.Results))
                .ForMember(x => x.LastPage, opt => opt.ResolveUsing<LastPageValueResolver>().FromMember(z => z.Results))
                .AfterMap((src, dest) => dest.Hits.ForEach(m => m.IsShortlisted =
                                src.Shortlist?.ProvidersIdAndLocation?.Any(x =>
                                    x.LocationId.Equals(m.LocationId) &&
                                    x.ProviderId.Equals(m.UkPrn, StringComparison.Ordinal)) ?? false));

            // ToDo: CF ->  Rename models?
            cfg.CreateMap<FrameworkProviderSearchResponse, ProviderFrameworkSearchResultViewModel>()
                .ForMember(x => x.AbsolutePath, y => y.Ignore())
                .ForMember(x => x.ActualPage, y => y.MapFrom(z => z.CurrentPage))
                .ForMember(x => x.Hits, y => y.MapFrom(z => z.Results.Hits))
                .ForMember(x => x.PostCode, y => y.MapFrom(z => z.Results.PostCode))
                .ForMember(x => x.PostCodeMissing, y => y.MapFrom(z => z.Results.PostCodeMissing))
                .ForMember(x => x.ResultsToTake, y => y.MapFrom(z => z.Results.ResultsToTake))
                .ForMember(x => x.Title, y => y.MapFrom(z => z.Results.Title))
                .ForMember(x => x.FrameworkLevel, y => y.MapFrom(z => z.Results.FrameworkLevel))
                .ForMember(x => x.ShowAll, y => y.MapFrom(z => z.ShowAllProviders))
                .ForMember(x => x.FrameworkCode, y => y.MapFrom(z => z.Results.FrameworkCode))
                .ForMember(x => x.FrameworkId, y => y.MapFrom(z => z.Results.FrameworkId))
                .ForMember(x => x.FrameworkName, y => y.MapFrom(z => z.Results.FrameworkName))
                .ForMember(x => x.PathwayName, y => y.MapFrom(z => z.Results.PathwayName))
                .ForMember(x => x.TotalResults, y => y.MapFrom(z => z.Results.TotalResults))
                .ForMember(x => x.HasError, y => y.MapFrom(z => !z.Success))
                .ForMember(x => x.TotalProvidersCountry, y => y.MapFrom(z => z.TotalResultsForCountry))
                .ForMember(x => x.DeliveryModes, opt => opt.ResolveUsing<DeliveryModesValueResolver>().FromMember(z => z.Results))
                .ForMember(x => x.LastPage, opt => opt.ResolveUsing<LastPageValueResolver>().FromMember(z => z.Results))
                .AfterMap((src, dest) => dest.Hits.ForEach(m => m.IsShortlisted =
                                src.Shortlist?.ProvidersIdAndLocation?.Any(x =>
                                    x.LocationId.Equals(m.LocationId) &&
                                    x.ProviderId.Equals(m.UkPrn, StringComparison.Ordinal)) ?? false));
        }

        private MapperConfiguration Config()
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