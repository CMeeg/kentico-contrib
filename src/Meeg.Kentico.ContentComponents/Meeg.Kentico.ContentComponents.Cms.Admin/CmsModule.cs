using CMS;
using CMS.DataEngine;

[assembly:RegisterModule(typeof(Meeg.Kentico.ContentComponents.Cms.Admin.CmsModule))]

namespace Meeg.Kentico.ContentComponents.Cms.Admin
{
    /// <summary>
    /// Meeg.Kentico.ContentComponents.Cms.Admin CMS Module.
    /// </summary>
    public class CmsModule : Module
    {
        private const string ModuleName = "Meeg.Kentico.ContentComponents.Cms.Admin";

        public CmsModule()
            : base(ModuleName, true)
        {
        }
    }
}
