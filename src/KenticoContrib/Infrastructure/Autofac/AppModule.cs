using Autofac;
using Meeg.Configuration;

namespace KenticoContrib.Infrastructure.Autofac
{
    public class AppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConfigurationManagerAdapter>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<AppConfiguration>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
