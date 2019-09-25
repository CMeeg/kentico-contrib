using KenticoContrib.Content.Home;

namespace KenticoContrib.Content.Cms.Home
{
    public class HomeMappingProfile : CmsMappingProfile
    {
        public HomeMappingProfile()
        {
            RecognizePrefixes(GetCmsPrefixes(CMS.DocumentEngine.Types.KenticoContrib.Home.CLASS_NAME));

            CreateMap<CMS.DocumentEngine.Types.KenticoContrib.Home, HomePage>()
                .ForMember(dest => dest.Metadata, member => member.MapFrom(source => source.MetadataComponent));
        }
    }
}
