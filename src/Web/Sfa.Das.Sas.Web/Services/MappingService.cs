using System;
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
            catch (Exception exp)
            {
                _logger.Error($"Error mapping objects: {exp.Message}");
            }

            return tempDest;
        }

        private static MapperConfiguration Config()
        {
            return new MapperConfiguration(cfg =>
            {
                // Apprenticeship search listing  -> mix of standard and framework
                cfg.CreateMap<ApprenticeshipSearchResults, ApprenticeshipSearchResultViewModel>();
                cfg.CreateMap<ApprenticeshipSearchResultsItem, ApprenticeshipSearchResultItemViewModel>().AfterMap<ApprenticeshipSearchResultViewModelMappingAction>();

                // Standard detail page
                cfg.CreateMap<Standard, StandardViewModel>().AfterMap<StandardViewModelMappingAction>();

                // Framework detail page
                cfg.CreateMap<Framework, FrameworkViewModel>();

                // Provider search
                cfg.CreateMap<ProviderStandardSearchResults, ProviderStandardSearchResultViewModel>();
                cfg.CreateMap<ProviderFrameworkSearchResults, ProviderFrameworkSearchResultViewModel>();
                cfg.CreateMap<FrameworkProviderSearchResultsItem, FrameworkProviderResultItemViewModel>().AfterMap<FrameworkProviderResultItemViewModelMappingAction>();
                cfg.CreateMap<StandardProviderSearchResultsItem, ProviderResultItemViewModel>().AfterMap<StandardProviderResultItemViewModelMappingAction>();

                // Provider detail page
                cfg.CreateMap<Provider, ProviderViewModel>().AfterMap<ProviderViewModelMappingAction>();
            });
        }
    }
}