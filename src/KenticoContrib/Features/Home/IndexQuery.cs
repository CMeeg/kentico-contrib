using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using KenticoContrib.Content.Home;
using MediatR;

namespace KenticoContrib.Features.Home
{
    public class IndexQuery : IRequest<HomeViewModel>
    {
    }

    public class IndexQueryHandler : IRequestHandler<IndexQuery, HomeViewModel>
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public IndexQueryHandler(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        public async Task<HomeViewModel> Handle(IndexQuery request, CancellationToken cancellationToken)
        {
            HomePage homePage = await mediator.Send(new GetHomePageQuery(), cancellationToken);

            return mapper.Map<HomeViewModel>(homePage);
        }
    }
}