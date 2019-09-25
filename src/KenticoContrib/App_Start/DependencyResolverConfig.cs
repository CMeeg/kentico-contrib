using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using KenticoContrib.Content.Cms.Infrastructure;
using KenticoContrib.Infrastructure;

namespace KenticoContrib
{
    public class DependencyResolverConfig
    {
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            // Register application modules

            builder.RegisterModule<MvcRegistrationModule>();
            builder.RegisterModule<MediatrRegistrationModule>();
            builder.RegisterModule<AutoMapperRegistrationModule>();

            // Register external (from outside of this assembly) modules

            builder.RegisterModule<ContentRegistrationModule>();

            // Set dependency resolver

            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}