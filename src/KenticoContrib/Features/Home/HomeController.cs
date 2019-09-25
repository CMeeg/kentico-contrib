using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using KenticoContrib.Content.Home;
using MediatR;

namespace KenticoContrib.Features.Home
{
    public class HomeController : Controller
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public HomeController(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        public async Task<ActionResult> Index()
        {
            var homePageQuery = new GetHomePageQuery();
            HomePage homePage = await mediator.Send(homePageQuery);

            var viewModel = mapper.Map<HomeViewModel>(homePage);

            return View(viewModel);
        }
    }
}