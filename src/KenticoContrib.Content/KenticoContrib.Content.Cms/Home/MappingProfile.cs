using KenticoContrib.Content.Cms.Infrastructure.AutoMapper;
using KenticoContrib.Content.Home;

namespace KenticoContrib.Content.Cms.Home
{
    public class MappingProfile : CmsMappingProfile
    {
        public MappingProfile()
        {
            RecognizePrefixes(GetCmsPrefixes(CMS.DocumentEngine.Types.KenticoContrib.Home.CLASS_NAME));

            CreateMap<CMS.DocumentEngine.Types.KenticoContrib.Home, HomePage>();
        }
    }
}
