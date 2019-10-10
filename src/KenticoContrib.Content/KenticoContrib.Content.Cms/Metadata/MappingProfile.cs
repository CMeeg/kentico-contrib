using KenticoContrib.Content.Cms.Infrastructure.AutoMapper;
using KenticoContrib.Content.Metadata;

namespace KenticoContrib.Content.Cms.Metadata
{
    public class MappingProfile : CmsMappingProfile
    {
        public MappingProfile()
        {
            RecognizePrefixes(GetCmsPrefixes(
                CMS.DocumentEngine.Types.KenticoContrib.PageMetadata.CLASS_NAME,
                CMS.DocumentEngine.Types.KenticoContrib.OpenGraphMetadata.CLASS_NAME,
                CMS.DocumentEngine.Types.KenticoContrib.TwitterMetadata.CLASS_NAME
            ));

            CreateMap<CMS.DocumentEngine.Types.KenticoContrib.PageMetadata, PageMetadata>();
            CreateMap<CMS.DocumentEngine.Types.KenticoContrib.OpenGraphMetadata, OpenGraphMetadata>();
            CreateMap<CMS.DocumentEngine.Types.KenticoContrib.TwitterMetadata, TwitterMetadata>()
                .ForMember(dest => dest.Site, member => member.MapFrom(src => src.TwitterMetadataSite));
        }
    }
}
