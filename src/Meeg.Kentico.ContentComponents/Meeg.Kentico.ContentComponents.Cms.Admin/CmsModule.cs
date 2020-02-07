using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using CMS;
using CMS.Base;
using CMS.Modules;
using NuGet;
using Module = CMS.DataEngine.Module;

[assembly:RegisterModule(typeof(Meeg.Kentico.ContentComponents.Cms.Admin.CmsModule))]

namespace Meeg.Kentico.ContentComponents.Cms.Admin
{
    /// <summary>
    /// Meeg.Kentico.ContentComponents.Cms.Admin CMS Module.
    /// </summary>
    public class CmsModule : Module
    {
        private const string ModuleName = "Meeg.Kentico.ContentComponents.Cms.Admin";

        private static readonly string NuSpecFilePath = $"..\\Meeg.Kentico.ContentComponents\\{ModuleName}\\{ModuleName}.nuspec";

        private AssemblyInfoAccessor assemblyInfo;
        private AssemblyInfoAccessor AssemblyInfo => assemblyInfo ?? (assemblyInfo = new AssemblyInfoAccessor(Assembly.GetExecutingAssembly()));

        public CmsModule()
            : base(ModuleName, true)
        {
        }

        protected override void OnInit()
        {
            base.OnInit();

            ModulePackagingEvents.Instance.BuildNuSpecManifest.Before += ModifyNuSpecBuilder;
            ModulePackagingEvents.Instance.BuildNuSpecManifest.After += ModifyManifest;
        }

        private void ModifyNuSpecBuilder(object sender, BuildNuSpecManifestEventArgs e)
        {
            if (!ResourceIsModule(e.ResourceName))
            {
                return;
            }

            e.NuSpecBuilder.ReadmeFilePath = null;

            e.NuSpecBuilder.ModulePackageMetadata.Id = AssemblyInfo.Title;
            e.NuSpecBuilder.ModulePackageMetadata.Version = AssemblyInfo.InformationalVersion;
            e.NuSpecBuilder.ModulePackageMetadata.Description = AssemblyInfo.Description;
            e.NuSpecBuilder.ModulePackageMetadata.Authors = AssemblyInfo.Company;
        }

        private bool ResourceIsModule(string resourceName)
        {
            return resourceName.Equals(ModuleName, StringComparison.OrdinalIgnoreCase);
        }

        private void ModifyManifest(object sender, BuildNuSpecManifestEventArgs e)
        {
            if (!ResourceIsModule(e.ResourceName))
            {
                return;
            }

            Manifest nuSpecManifest = LoadNuSpecManifest();

            e.Manifest = MergeManifests(nuSpecManifest, e.Manifest);
        }

        private Manifest LoadNuSpecManifest()
        {
            using (FileStream nuspecFile = File.OpenRead(Path.Combine(SystemContext.WebApplicationPhysicalPath, NuSpecFilePath)))
            {
                var propertyProvider = new AssemblyInfoPropertyProvider(AssemblyInfo);

                return Manifest.ReadFrom(nuspecFile, propertyProvider, false);
            }
        }

        private class AssemblyInfoPropertyProvider : IPropertyProvider
        {
            private readonly AssemblyInfoAccessor assemblyInfo;

            private static readonly Dictionary<string, Func<AssemblyInfoAccessor, string>> Replacements = new Dictionary<string, Func<AssemblyInfoAccessor, string>>
            {
                { "id", assemblyInfo => assemblyInfo.Title },
                { "version", assemblyInfo => assemblyInfo.InformationalVersion },
                { "description", assemblyInfo => assemblyInfo.Description },
                { "author", assemblyInfo => assemblyInfo.Company },
                { "copyright", assemblyInfo => assemblyInfo.Copyright }
            };

            public AssemblyInfoPropertyProvider(AssemblyInfoAccessor assemblyInfo)
            {
                this.assemblyInfo = assemblyInfo;
            }

            public dynamic GetPropertyValue(string propertyName)
            {
                string key = propertyName.ToLower();

                if (!Replacements.ContainsKey(key))
                {
                    return null;
                }

                return Replacements[key].Invoke(assemblyInfo);
            }
        }

        private Manifest MergeManifests(Manifest nuSpecManifest, Manifest moduleManifest)
        {
            nuSpecManifest.Files = moduleManifest.Files;

            string readmeSource = $"CMSModules\\{ModuleName}\\readme.txt";

            nuSpecManifest.Files.Add(new ManifestFile
            {
                Source = readmeSource,
                Target = string.Empty
            });

            return nuSpecManifest;
        }
    }
}
