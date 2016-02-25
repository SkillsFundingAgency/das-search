namespace Sfa.Eds.Das.Web.Services
{
    using System;

    using AutoMapper;
    using Core.Domain.Model;
    using Sfa.Das.ApplicationServices.Models;
    using Sfa.Eds.Das.Core.Logging;
    using ViewModels;

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
                cfg.CreateMap<ProviderSearchResultsItem, ProviderResultItemViewModel>();
                cfg.CreateMap<ProviderSearchResultsItem, ProviderViewModel>();
                cfg.CreateMap<ProviderSearchResults, ProviderSearchResultViewModel>();
                cfg.CreateMap<StandardSearchResultsItem, StandardResultItemViewModel>();
                cfg.CreateMap<StandardSearchResultsItem, StandardViewModel>();
                cfg.CreateMap<StandardSearchResults, StandardSearchResultViewModel>();
                cfg.CreateMap<StandardDetail, StandardViewModel>();
            });
        }
    }
}