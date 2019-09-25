using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace KenticoContrib.Content.Cms.Infrastructure
{
    public class SetCurrentPagePipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ICurrentPageContext currentPageContext;

        public SetCurrentPagePipelineBehaviour(ICurrentPageContext currentPageContext)
        {
            this.currentPageContext = currentPageContext;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = await next();

            if (typeof(IPage).IsAssignableFrom(typeof(TResponse)))
            {
                currentPageContext.SetCurrentPage(response as IPage);
            }

            return response;
        }
    }
}
