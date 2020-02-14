using System;
using System.Reflection;

namespace Meeg.Kentico.ContentComponents.Cms.Admin
{
    internal class AssemblyInfoAccessor
    {
        private readonly Assembly assembly;

        public AssemblyInfoAccessor(Assembly assembly)
        {
            this.assembly = assembly;
        }

        public string Title => GetAttribute<AssemblyTitleAttribute>()?.Title;

        public string Description => GetAttribute<AssemblyDescriptionAttribute>()?.Description;

        public string Company => GetAttribute<AssemblyCompanyAttribute>()?.Company;

        public string Copyright => GetAttribute<AssemblyCopyrightAttribute>()?.Copyright;

        public string InformationalVersion => GetAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

        private T GetAttribute<T>()
            where T : Attribute
        {
            // Get attributes of type T

            object[] attributes = assembly.GetCustomAttributes(typeof(T), true);

            if (attributes.Length == 0)
            {
                // No attributes of type T found

                return null;
            }

            // Return the first attribute found as type T

            return attributes[0] as T;
        }
    }
}
