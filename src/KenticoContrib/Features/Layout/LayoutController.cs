using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using MediatR;
using Meeg.Configuration;
using Meeg.Kentico.Configuration.Cms;

namespace KenticoContrib.Features.Layout
{
    public class LayoutController : Controller
    {
        private readonly IMediator mediator;
        private readonly IAppConfigurationRoot appConfig;

        public LayoutController(IMediator mediator, IAppConfigurationRoot appConfig)
        {
            this.mediator = mediator;
            this.appConfig = appConfig;
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

        [ChildActionOnly]
        public ActionResult Favicon()
        {
            // TODO: Get current site name rather than hardcode

            string currentSiteName = "KenticoContrib";

            var faviconOptions = appConfig
                .Get<FaviconOptions>("KenticoContrib");

            string faviconPath = faviconOptions.CmsFaviconPath;

            if (string.IsNullOrEmpty(faviconPath))
            {
                return new EmptyResult();
            }

            string faviconHtml = $"<link rel=\"shortcut icon\" href=\"{faviconPath}\">";

            return new ContentResult
            {
                Content = faviconHtml,
                ContentType = "text/html",
                ContentEncoding = Encoding.UTF8
            };
        }

        private class FaviconOptions
        {
            public string CmsFaviconPath { get; set; }
        }
    }
}
