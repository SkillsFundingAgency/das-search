namespace Sfa.Eds.Das.Web.Services
{
    using System;
    using AutoMapper;
    using Core.Models;
    using log4net;
    using ViewModels;

    public class MappingService : IMappingService
    {
        private static IMapper mapper;
        private readonly ILog logger;

        public MappingService(ILog logger)
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
                cfg.CreateMap<SearchResultsItem, StandardResultItemViewModel>();
                cfg.CreateMap<SearchResultsItem, StandardViewModel>();
                cfg.CreateMap<SearchResults, StandardSearchResultViewModel>();
            });
        }
    }
}