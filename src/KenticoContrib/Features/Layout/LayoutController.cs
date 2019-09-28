using System.Threading.Tasks;
using System.Web.Mvc;
using MediatR;

namespace KenticoContrib.Features.Layout
{
    public class LayoutController : Controller
    {
        private readonly IMediator mediator;

        public LayoutController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [ChildActionOnly]
        public async Task<ActionResult> PageMetadata()
        {
            PageMetadataViewModel viewModel = await mediator.Send(new PageMetadataQuery());

            if (viewModel == null)
            {
                return new EmptyResult();
            }

            return PartialView("_PageMetadata", viewModel);
        }
    }
}