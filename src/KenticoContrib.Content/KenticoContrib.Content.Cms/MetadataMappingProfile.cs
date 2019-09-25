using CMS.DocumentEngine.Types.KenticoContrib;

namespace KenticoContrib.Content.Cms
{
    public class MetadataMappingProfile : CmsMappingProfile
    {
        public MetadataMappingProfile()
        {
            RecognizePrefixes(GetCmsPrefixes(Metadata.CLASS_NAME));

            CreateMap<Metadata, PageMetadata>();
        }
    }
}
