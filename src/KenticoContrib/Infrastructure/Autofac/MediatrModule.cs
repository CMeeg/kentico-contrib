using Autofac;
using MediatR;
using MediatR.Pipeline;

namespace KenticoContrib.Infrastructure.Autofac
{
    public class MediatrModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterMediatr(builder);
        }

        private void RegisterMediatr(ContainerBuilder builder)
        {
            // See https://github.com/jbogard/MediatR/wiki#autofac
            // and https://github.com/jbogard/MediatR/blob/master/samples/MediatR.Examples.Autofac/Program.cs

            builder.RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));

            builder.Register<ServiceFactory>(context =>
            {
                var componentContext = context.Resolve<IComponentContext>();
                return serviceType => componentContext.Resolve(serviceType);
            });
        }
    }
}