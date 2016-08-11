using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Logging;
using Sfa.Das.Sas.Core.Models;

namespace Sfa.Das.Sas.ApplicationServices.Services
{
    public class MappingService : IMappingService
    {
        private readonly IMapper _mapper;

        public MappingService()
        {
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
                throw;
            }
        }

        private static void CreateFrameworkMetaDataMappings(IMapperConfiguration cfg)
        {
            cfg.CreateMap<VstsFrameworkMetaData, FrameworkMetaData>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.EntryRequirements, opt => opt.MapFrom(source => source.EntryRequirements))
                .ForMember(dest => dest.FrameworkCode, opt => opt.MapFrom(source => source.FrameworkCode))
                .ForMember(dest => dest.FrameworkOverview, opt => opt.MapFrom(source => source.FrameworkOverview))
                .ForMember(dest => dest.JobRoleItems, opt => opt.MapFrom(source => source.JobRoleItems))
                .ForMember(dest => dest.Keywords, opt => opt.MapFrom(source => source.Keywords))
                .ForMember(dest => dest.PathwayCode, opt => opt.MapFrom(source => source.PathwayCode))
                .ForMember(dest => dest.ProfessionalRegistration, opt => opt.MapFrom(source => source.ProfessionalRegistration))
                .ForMember(dest => dest.ProgType, opt => opt.MapFrom(source => source.ProgType))
                .ForMember(dest => dest.TypicalLength, opt => opt.MapFrom(source => source.TypicalLength))
                .ForMember(dest => dest.CompletionQualifications, opt => opt.MapFrom(source => source.CompletionQualifications))
                .ForMember(dest => dest.Pathway, opt => opt.MapFrom(source => source.Pathway))
                .ForMember(dest => dest.FrameworkName, opt => opt.MapFrom(source => source.FrameworkName))
                .ForMember(dest => dest.CombinedQualification, opt => opt.Ignore())
                .ForMember(dest => dest.CompetencyQualification, opt => opt.Ignore())
                .ForMember(dest => dest.EffectiveFrom, opt => opt.Ignore())
                .ForMember(dest => dest.EffectiveTo, opt => opt.Ignore())
                .ForMember(dest => dest.KnowledgeQualification, opt => opt.Ignore())
                .ForMember(dest => dest.NasTitle, opt => opt.Ignore())
                .ForMember(dest => dest.SectorSubjectAreaTier1, opt => opt.Ignore())
                .ForMember(dest => dest.SectorSubjectAreaTier2, opt => opt.Ignore())
                ;
        }

        private static MapperConfiguration Config()
        {
            return new MapperConfiguration(cfg =>
            {
                CreateFrameworkMetaDataMappings(cfg);
            });
        }
    }
}
