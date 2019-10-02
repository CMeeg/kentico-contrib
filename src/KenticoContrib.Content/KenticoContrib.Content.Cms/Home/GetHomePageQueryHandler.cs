using System.Linq;
using AutoMapper;
using KenticoContrib.Content.Cms.Infrastructure.Cms;
using KenticoContrib.Content.Home;
using MediatR;

namespace KenticoContrib.Content.Cms.Home
{
    public class GetHomePageQueryHandler : RequestHandler<GetHomePageQuery, HomePage>
    {
        private readonly IMapper mapper;
        private readonly DocumentQueryService documentQueryService;

        public GetHomePageQueryHandler(IMapper mapper, DocumentQueryService documentQueryService)
        {
            this.mapper = mapper;
            this.documentQueryService = documentQueryService;
        }

        protected override HomePage Handle(GetHomePageQuery request)
        {
            // TODO: Caching

            var node = documentQueryService.GetQuery<CMS.DocumentEngine.Types.KenticoContrib.Home>()
                .Columns(
                    nameof(CMS.DocumentEngine.Types.KenticoContrib.Home.HomeMetadata)
                )
                .AddColumns(ColumnDefinitions.IPageColumns)
                .ToList()
                .FirstOrDefault();

            if (node == null)
            {
                return null;
            }

            var homePage = mapper.Map<HomePage>(node);

            return homePage;
        }
    }
}
