using Autofac;
using AutoMapper;
using KenticoContrib.Content.Cms.Infrastructure.Mediatr;
using MediatR;

namespace KenticoContrib.Content.Cms.Infrastructure.Autofac
{
    public class ContentModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterMediatrComponents(builder);

            RegisterAutoMapperComponents(builder);

            builder.RegisterType<CurrentPageContext>()
                .As<ICurrentPageContext>()
                .InstancePerLifetimeScope();
        }

        private void RegisterMediatrComponents(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>))
                .AsImplementedInterfaces();

            builder.RegisterGeneric(typeof(SetCurrentPagePipelineBehaviour<,>)).As(typeof(IPipelineBehavior<,>));
        }

        private void RegisterAutoMapperComponents(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(type => typeof(Profile).IsAssignableFrom(type) && !type.IsAbstract && type.IsPublic)
                .As<Profile>();
        }
    }
}
