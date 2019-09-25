using Autofac;
using Autofac.Integration.Mvc;

namespace KenticoContrib.Infrastructure
{
    public class MvcRegistrationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // See https://autofaccn.readthedocs.io/en/latest/integration/mvc.html#mvc

            builder.RegisterControllers(ThisAssembly);

            builder.RegisterModelBinders(ThisAssembly);
            builder.RegisterModelBinderProvider();

            builder.RegisterModule<AutofacWebTypesModule>();

            builder.RegisterFilterProvider();
        }
    }
}