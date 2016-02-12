namespace Sfa.Eds.Das.Web.Services
{
    using System;

    using AutoMapper;
    using Core.Models;

    using Sfa.Das.ApplicationServices.Models;
    using Sfa.Eds.Das.Core.Logging;
    using Sfa.Eds.Das.Infrastructure.Logging;

    using ViewModels;

    public class MappingService : IMappingService
    {
        private static IMapper mapper;
        private readonly IApplicationLogger logger;

        public MappingService(IApplicationLogger logger)
        {
            this.logger = logger;
        }

        private static IMapper Mapper
        {
            get
            {
                if (mapper == null)
                {
                    var config = Config();
                    mapper = config.CreateMapper();
                }

                return mapper;
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
                logger.Error($"Error mapping objects: {exp.Message}");
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
                cfg.CreateMap<Standard, StandardViewModel>();
            });
        }
    }
}