using KenticoContrib.Content.Cms.Infrastructure.AutoMapper;
using KenticoContrib.Content.Metadata;

namespace KenticoContrib.Content.Cms.Metadata
{
    public class MappingProfile : CmsMappingProfile
    {
        public MappingProfile()
        {
            RecognizePrefixes(GetCmsPrefixes(CMS.DocumentEngine.Types.KenticoContrib.Metadata.CLASS_NAME));

            CreateMap<CMS.DocumentEngine.Types.KenticoContrib.Metadata, PageMetadata>();
        }
    }
}
