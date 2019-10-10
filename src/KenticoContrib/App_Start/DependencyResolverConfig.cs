using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using KenticoContrib.Content.Cms.Infrastructure.Autofac;
using KenticoContrib.Infrastructure.Autofac;

namespace KenticoContrib
{
    public class DependencyResolverConfig
    {
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            // Register application modules

            builder.RegisterModule<MvcModule>();
            builder.RegisterModule<MediatrModule>();
            builder.RegisterModule<AutoMapperModule>();

            // Register external (from outside of this assembly) modules

            builder.RegisterModule<ContentModule>();

            // Set dependency resolver

            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}