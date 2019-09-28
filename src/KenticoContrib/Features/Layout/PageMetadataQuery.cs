using AutoMapper;
using KenticoContrib.Content;
using MediatR;

namespace KenticoContrib.Features.Layout
{
    public class PageMetadataQuery : IRequest<PageMetadataViewModel>
    {
    }

    public class PageMetadataQueryHandler : RequestHandler<PageMetadataQuery, PageMetadataViewModel>
    {
        private readonly ICurrentPageContext currentPageContext;
        private readonly IMapper mapper;

        public PageMetadataQueryHandler(ICurrentPageContext currentPageContext, IMapper mapper)
        {
            this.currentPageContext = currentPageContext;
            this.mapper = mapper;
        }

        protected override PageMetadataViewModel Handle(PageMetadataQuery request)
        {
            IPage page = currentPageContext.Page;

            if (page == null)
            {
                return null;
            }

            var viewModel = mapper.Map<PageMetadataViewModel>(page.Metadata ?? new Content.Metadata.PageMetadata());

            EnsureRequiredPageMetadataIsPresent(viewModel, page);

            return viewModel;
        }

        private void EnsureRequiredPageMetadataIsPresent(PageMetadataViewModel viewModel, IPage page)
        {
            if (string.IsNullOrEmpty(viewModel.PageTitle))
            {
                viewModel.PageTitle = page.Name;
            }
        }
    }
}