using Autofac;
using ITG.Brix.Users.Application.Behaviors;
using MediatR;

namespace ITG.Brix.Users.DependencyResolver.AutofacModules
{
    public class BehaviorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(ValidatorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        }
    }
}

