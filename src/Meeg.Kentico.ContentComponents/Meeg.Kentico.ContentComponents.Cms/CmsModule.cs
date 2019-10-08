using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CMS;
using CMS.Base;
using CMS.Modules;
using NuGet;
using Module = CMS.DataEngine.Module;

[assembly:RegisterModule(typeof(Meeg.Kentico.ContentComponents.Cms.CmsModule))]

namespace Meeg.Kentico.ContentComponents.Cms
{
    /// <summary>
    /// Meeg.Kentico.ContentComponents.Cms CMS Module.
    /// </summary>
    public class CmsModule : Module
    {
        private const string ModuleName = "Meeg.Kentico.ContentComponents.Cms";

        private static readonly string NuSpecFilePath = $"..\\Meeg.Kentico.ContentComponents\\{ModuleName}\\{ModuleName}.nuspec";

        private AssemblyInfo assemblyInfo;
        private AssemblyInfo AssemblyInfo => assemblyInfo ?? (assemblyInfo = new AssemblyInfo(Assembly.GetExecutingAssembly()));

        public CmsModule()
            : base(ModuleName, true)
        {
        }

        protected override void OnInit()
        {
            base.OnInit();

            ModulePackagingEvents.Instance.BuildNuSpecManifest.After += ModifyManifest;
        }

        private void ModifyManifest(object sender, BuildNuSpecManifestEventArgs e)
        {
            if (!e.ResourceName.Equals(ModuleName, StringComparison.OrdinalIgnoreCase))
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
            private readonly AssemblyInfo assemblyInfo;

            private static readonly Dictionary<string, Func<AssemblyInfo, string>> Replacements = new Dictionary<string, Func<AssemblyInfo, string>>
            {
                { "id", assemblyInfo => assemblyInfo.Title },
                { "version", assemblyInfo => assemblyInfo.InformationalVersion },
                { "description", assemblyInfo => assemblyInfo.Description },
                { "author", assemblyInfo => assemblyInfo.Company },
                { "copyright", assemblyInfo => assemblyInfo.Copyright }
            };

            public AssemblyInfoPropertyProvider(AssemblyInfo assemblyInfo)
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
            string readmeSource = $"CMSModules\\{ModuleName}\\README.md";
            const string readmeTarget = "readme.txt";

            List<ManifestFile> files = moduleManifest.Files
                .Where(file => file.Target != readmeTarget)
                .ToList();

            files.Add(new ManifestFile
            {
                Source = readmeSource,
                Target = string.Empty
            });

            nuSpecManifest.Files = files;

            return nuSpecManifest;
        }
    }
}
