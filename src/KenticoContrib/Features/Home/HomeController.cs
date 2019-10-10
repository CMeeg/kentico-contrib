using System.Threading.Tasks;
using System.Web.Mvc;
using MediatR;

namespace KenticoContrib.Features.Home
{
    public class HomeController : Controller
    {
        private readonly IMediator mediator;

        public HomeController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<ActionResult> Index()
        {
            HomeViewModel viewModel = await mediator.Send(new IndexQuery());

            return View(viewModel);
        }
    }
}