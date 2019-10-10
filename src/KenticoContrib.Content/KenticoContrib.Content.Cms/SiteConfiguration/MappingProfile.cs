using KenticoContrib.Content.Cms.Infrastructure.AutoMapper;

namespace KenticoContrib.Content.Cms.SiteConfiguration
{
    public class MappingProfile : CmsMappingProfile
    {
        public MappingProfile()
        {
            RecognizePrefixes(GetCmsPrefixes(CMS.DocumentEngine.Types.KenticoContrib.SiteConfiguration.CLASS_NAME));

            CreateMap<CMS.DocumentEngine.Types.KenticoContrib.SiteConfiguration, Content.SiteConfiguration.SiteConfiguration>();
        }
    }
}
