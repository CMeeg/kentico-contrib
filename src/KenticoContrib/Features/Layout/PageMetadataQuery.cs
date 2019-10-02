using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using KenticoContrib.Content;
using KenticoContrib.Content.Metadata;
using KenticoContrib.Content.SiteConfiguration;
using MediatR;

namespace KenticoContrib.Features.Layout
{
    public class PageMetadataQuery : IRequest<PageMetadataViewModel>
    {
    }

    public class PageMetadataQueryHandler : IRequestHandler<PageMetadataQuery, PageMetadataViewModel>
    {
        private readonly ICurrentPageContext currentPageContext;
        private readonly IMapper mapper;
        private readonly IMediator mediator;

        public PageMetadataQueryHandler(ICurrentPageContext currentPageContext, IMapper mapper, IMediator mediator)
        {
            this.currentPageContext = currentPageContext;
            this.mapper = mapper;
            this.mediator = mediator;
        }

        public async Task<PageMetadataViewModel> Handle(PageMetadataQuery request, CancellationToken cancellationToken)
        {
            IPage page = currentPageContext.Page;

            if (page == null)
            {
                return null;
            }

            SiteConfiguration siteConfig = await mediator.Send(new GetSiteConfigurationQuery(), cancellationToken);

            PageMetadata pageMetadata = EnsureRequiredPageMetadata(page, siteConfig);

            var viewModel = mapper.Map<PageMetadataViewModel>(pageMetadata);

            return viewModel;
        }

        private PageMetadata EnsureRequiredPageMetadata(IPage page, SiteConfiguration siteConfig)
        {
            // Page metadata

            var pageMetadata = page.Metadata ?? new PageMetadata();
            
            if (string.IsNullOrEmpty(pageMetadata.PageTitle))
            {
                pageMetadata.PageTitle = page.Name;
            }

            // Open Graph metadata

            var openGraphMetadata = pageMetadata.OpenGraph;
            if (openGraphMetadata == null)
            {
                openGraphMetadata = pageMetadata.OpenGraph = new OpenGraphMetadata();
            }

            if (string.IsNullOrEmpty(openGraphMetadata.Title))
            {
                openGraphMetadata.Title = pageMetadata.PageTitle;
            }

            if (string.IsNullOrEmpty(openGraphMetadata.Description))
            {
                openGraphMetadata.Description = pageMetadata.PageDescription;
            }

            if (string.IsNullOrEmpty(openGraphMetadata.SiteName))
            {
                // TODO: If openGraphMetadata.SiteName is still null/empty use current site display name

                openGraphMetadata.SiteName = siteConfig?.DefaultMetadata?.OpenGraph.SiteName;
            }

            if (string.IsNullOrEmpty(openGraphMetadata.Url))
            {
                openGraphMetadata.Url = page.RelativeUrl;
            }

            if (string.IsNullOrEmpty(openGraphMetadata.Image))
            {
                openGraphMetadata.Image = siteConfig?.DefaultMetadata?.OpenGraph.Image;
            }

            if (string.IsNullOrEmpty(openGraphMetadata.ImageAltText))
            {
                openGraphMetadata.ImageAltText = siteConfig?.DefaultMetadata?.OpenGraph.ImageAltText;
            }

            // Twitter metadata

            var twitterMetadata = pageMetadata.Twitter;
            if (twitterMetadata == null)
            {
                twitterMetadata = pageMetadata.Twitter = new TwitterMetadata();
            }

            if (string.IsNullOrEmpty(twitterMetadata.Site))
            {
                twitterMetadata.Site = siteConfig?.DefaultMetadata?.Twitter?.Site;
            }

            // Add page title suffix

            if (!string.IsNullOrEmpty(pageMetadata.PageTitleSuffix))
            {
                pageMetadata.PageTitle = $"{pageMetadata.PageTitle} - {pageMetadata.PageTitleSuffix}";
            }

            return pageMetadata;
        }
    }
}