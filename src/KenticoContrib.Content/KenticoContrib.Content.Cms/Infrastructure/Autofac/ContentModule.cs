using Autofac;
using AutoMapper;
using KenticoContrib.Content.Cms.Infrastructure.Cms;
using KenticoContrib.Content.Cms.Infrastructure.Mediatr;
using MediatR;
using MediatR.Pipeline;

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

            builder.RegisterType<DocumentQueryService>()
                .AsSelf();
        }

        private void RegisterMediatrComponents(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>))
                .AsImplementedInterfaces();

            builder.RegisterGeneric(typeof(SetCurrentPageRequestPostProcessor<,>)).As(typeof(IRequestPostProcessor<,>));
        }

        private void RegisterAutoMapperComponents(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(type => typeof(Profile).IsAssignableFrom(type) && !type.IsAbstract && type.IsPublic)
                .As<Profile>();
        }
    }
}
