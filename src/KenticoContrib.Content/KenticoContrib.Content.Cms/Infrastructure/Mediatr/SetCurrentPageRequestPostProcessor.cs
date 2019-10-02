using System.Threading;
using System.Threading.Tasks;
using MediatR.Pipeline;

namespace KenticoContrib.Content.Cms.Infrastructure.Mediatr
{
    public class SetCurrentPageRequestPostProcessor<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse>
        where TResponse : IPage
    {
        private readonly ICurrentPageContext currentPageContext;

        public SetCurrentPageRequestPostProcessor(ICurrentPageContext currentPageContext)
        {
            this.currentPageContext = currentPageContext;
        }

        public Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
        {
            currentPageContext.SetCurrentPage(response);

            return Task.CompletedTask;
        }
    }
}
