using Autofac;
using MediatR;
using System.Collections.Generic;
using System.Reflection;

namespace ITG.Brix.Users.DependencyResolver.AutofacModules
{
    public class MediatorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterMediator(builder);
            RegisterRequestHandlers(builder);
            RegisterNotificationHandlers(builder);
        }

        private void RegisterMediator(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();
        }

        private void RegisterRequestHandlers(ContainerBuilder builder)
        {
            builder.Register<SingleInstanceFactory>(context =>
            {
                var componentContext = context.Resolve<IComponentContext>();
                return t => { object o; return componentContext.TryResolve(t, out o) ? o : null; };
            });
        }

        private void RegisterNotificationHandlers(ContainerBuilder builder)
        {
            builder.Register<MultiInstanceFactory>(context =>
            {
                var componentContext = context.Resolve<IComponentContext>();

                return t =>
                {
                    var resolved = (IEnumerable<object>)componentContext.Resolve(typeof(IEnumerable<>).MakeGenericType(t));
                    return resolved;
                };
            });
        }
    }
}
