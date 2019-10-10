using System.Collections.Generic;
using Autofac;
using AutoMapper;

namespace KenticoContrib.Infrastructure.Autofac
{
    public class AutoMapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterAutoMapper(builder);

            RegisterAutoMapperComponents(builder);
        }

        private void RegisterAutoMapper(ContainerBuilder builder)
        {
            // See https://automapper.readthedocs.io/en/latest/Dependency-injection.html#autofac
            // and https://protomatter.co.uk/blog/development/2017/02/modular-automapper-registrations-with-autofac/

            // Register config as singleton

            builder.Register<IConfigurationProvider>(ctx => new MapperConfiguration(cfg =>
            {
                // Resolve profiles that have been registered with the container and add them to the config

                var profiles = ctx.Resolve<IEnumerable<Profile>>();
                cfg.AddProfiles(profiles);
            }));

            // Register mapper as transient

            builder.Register<IMapper>(ctx => new Mapper(
                ctx.Resolve<IConfigurationProvider>(),
                ctx.Resolve)
            ).InstancePerDependency();
        }

        private void RegisterAutoMapperComponents(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(type => typeof(Profile).IsAssignableFrom(type) && !type.IsAbstract && type.IsPublic)
                .As<Profile>();
        }
    }
}