using System.Linq;
using AutoMapper;
using KenticoContrib.Content.Cms.Infrastructure.Cms;
using KenticoContrib.Content.SiteConfiguration;
using MediatR;

namespace KenticoContrib.Content.Cms.SiteConfiguration
{
    public class GetSiteConfigurationQueryHandler : RequestHandler<GetSiteConfigurationQuery, Content.SiteConfiguration.SiteConfiguration>
    {
        private readonly IMapper mapper;
        private readonly DocumentQueryService documentQueryService;

        public GetSiteConfigurationQueryHandler(IMapper mapper, DocumentQueryService documentQueryService)
        {
            this.mapper = mapper;
            this.documentQueryService = documentQueryService;
        }

        protected override Content.SiteConfiguration.SiteConfiguration Handle(GetSiteConfigurationQuery request)
        {
            // TODO: Caching

            var node = documentQueryService.GetQuery<CMS.DocumentEngine.Types.KenticoContrib.SiteConfiguration>()
                .Columns(
                    nameof(CMS.DocumentEngine.Types.KenticoContrib.SiteConfiguration.SiteConfigurationDefaultMetadata)
                )
                .ToList()
                .FirstOrDefault();

            if (node == null)
            {
                return null;
            }

            var siteConfig = mapper.Map<Content.SiteConfiguration.SiteConfiguration>(node);

            return siteConfig;
        }
    }
}
