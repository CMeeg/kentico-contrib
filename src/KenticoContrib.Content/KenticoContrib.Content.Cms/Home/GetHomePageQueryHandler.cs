using System.Linq;
using AutoMapper;
using CMS.DocumentEngine;
using KenticoContrib.Content.Home;
using MediatR;

namespace KenticoContrib.Content.Cms.Home
{
    public class GetHomePageQueryHandler : RequestHandler<GetHomePageQuery, HomePage>
    {
        private readonly IMapper mapper;

        public GetHomePageQueryHandler(IMapper mapper)
        {
            this.mapper = mapper;
        }

        protected override HomePage Handle(GetHomePageQuery request)
        {
            CMS.DocumentEngine.Types.KenticoContrib.Home homeNode = DocumentHelper.GetDocuments<CMS.DocumentEngine.Types.KenticoContrib.Home>()
                .TopN(1)
                .Columns(
                    nameof(CMS.DocumentEngine.Types.KenticoContrib.Home.DocumentID),
                    nameof(CMS.DocumentEngine.Types.KenticoContrib.Home.DocumentName),
                    nameof(CMS.DocumentEngine.Types.KenticoContrib.Home.HomeMetadata)
                )
                .Culture("en-gb") // TODO: Don't hardcode this
                .ToList()
                .FirstOrDefault();

            if (homeNode == null)
            {
                return null;
            }

            var homePage = mapper.Map<HomePage>(homeNode);

            return homePage;
        }
    }
}
