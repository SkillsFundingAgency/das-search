using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Core.Logging;
using Sfa.Das.Sas.Web.Services.MappingActions;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Services
{
    public class MappingService : IMappingService
    {
        private static IMapper _mapper;
        private readonly ILog _logger;

        public MappingService(ILog logger)
        {
            _logger = logger;
        }

        private static IMapper Mapper
        {
            get
            {
                if (_mapper == null)
                {
                    var config = Config();
                    _mapper = config.CreateMapper();
                }

                return _mapper;
            }
        }

        public TDest Map<TSource, TDest>(TSource source)
        {
            TDest tempDest = default(TDest);
            try
            {
                tempDest = Mapper.Map<TSource, TDest>(source);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error mapping objects");
            }

            return tempDest;
        }

        private static MapperConfiguration Config()
        {
            return new MapperConfiguration(cfg =>
            {
                // Apprenticeship search listing  -> mix of standard and framework
                cfg.CreateMap<ApprenticeshipSearchResults, ApprenticeshipSearchResultViewModel>().AfterMap<ApprenticeshipSearchResultViewModelMappingAction>();
                cfg.CreateMap<ApprenticeshipSearchResultsItem, ApprenticeshipSearchResultItemViewModel>().AfterMap<ApprenticeshipSearchResultItemViewModelMappingAction>();

                // Standard detail page
                cfg.CreateMap<Standard, StandardViewModel>().AfterMap<StandardViewModelMappingAction>();

                // Framework detail page
                cfg.CreateMap<Framework, FrameworkViewModel>();

                // Provider search
                cfg.CreateMap<ProviderStandardSearchResults, ProviderStandardSearchResultViewModel>().AfterMap<ProviderStandardSearchResultViewModelMappingAction>();
                cfg.CreateMap<ProviderFrameworkSearchResults, ProviderFrameworkSearchResultViewModel>().AfterMap<ProviderFrameworkSearchResultViewModelMappingAction>();
                cfg.CreateMap<FrameworkProviderSearchResultsItem, FrameworkProviderResultItemViewModel>().AfterMap<FrameworkProviderResultItemViewModelMappingAction>();
                

                cfg.CreateMap<StandardProviderSearchResultsItem, ProviderResultItemViewModel>().AfterMap<StandardProviderResultItemViewModelMappingAction>();

                // Provider detail page
                cfg.CreateMap<ApprenticeshipDetails, ApprenticeshipDetailsViewModel>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(source => source.Location.Address))
                .ForMember(dest => dest.Apprenticeship, opt => opt.MapFrom(source => source.Product.Apprenticeship))
                .ForMember(dest => dest.ContactInformation, opt => opt.MapFrom(source => source.Provider.ContactInformation))
                .ForMember(dest => dest.DeliveryModes, opt => opt.MapFrom(source => source.Product.DeliveryModes))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(source => source.Location))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.Provider.Name))
                .ForMember(dest => dest.ProviderId, opt => opt.MapFrom(source => source.Provider.Id))
                .ForMember(dest => dest.EmployerSatisfaction, opt => opt.MapFrom(source => source.Product.EmployerSatisfaction))
                .ForMember(dest => dest.LearnerSatisfaction, opt => opt.MapFrom(source => source.Product.LearnerSatisfaction))
                .ForMember(dest => dest.ProviderMarketingInfo, opt => opt.MapFrom(source => source.Product.ProviderMarketingInfo))
                .ForMember(dest => dest.ProviderId, opt => opt.MapFrom(source => source.Provider.Id))
                .AfterMap<ProviderViewModelMappingAction>();
            });
        }
    }
}