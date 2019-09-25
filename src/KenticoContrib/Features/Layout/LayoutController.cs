using System.Web.Mvc;
using AutoMapper;
using KenticoContrib.Content;

namespace KenticoContrib.Features.Layout
{
    public class LayoutController : Controller
    {
        private readonly ICurrentPageContext currentPageContext;
        private readonly IMapper mapper;

        public LayoutController(ICurrentPageContext currentPageContext, IMapper mapper)
        {
            this.currentPageContext = currentPageContext;
            this.mapper = mapper;
        }

        [ChildActionOnly]
        public ActionResult PageMetadata()
        {
            IPage page = currentPageContext.Page;

            if (page == null)
            {
                return new EmptyResult();
            }

            var viewModel = mapper.Map<PageMetadataViewModel>(page.Metadata);

            EnsureRequiredPageMetadataIsPresent(viewModel, page);

            return PartialView("_PageMetadata", viewModel);
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