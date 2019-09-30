using Autofac;
using Autofac.Extensions.DependencyInjection;
using ITG.Brix.Users.DependencyResolver.AutofacModules;
using Microsoft.Extensions.DependencyInjection;

namespace ITG.Brix.Users.DependencyResolver
{
    public static class Resolver
    {
        public static ContainerBuilder BuildServiceProvider(IServiceCollection services, string connectionString)
        {
            services
                .AutoMapper()
                .Persistence(connectionString)
                .Providers()
                .ApiServices();

            var containerBuilder = BuildContainer(services);
            return containerBuilder;
        }

        private static ContainerBuilder BuildContainer(IServiceCollection services)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);
            containerBuilder
                .RegisterModule(new MediatorModule())
                .RegisterModule(new CommandHandlerModule())
                .RegisterModule(new ValidatorModule())
                .RegisterModule(new BehaviorModule());

            return containerBuilder;
        }
    }
}
