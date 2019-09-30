using Autofac;
using ITG.Brix.Users.Application.Cqs.Commands;
using MediatR;
using System.Reflection;

namespace ITG.Brix.Users.DependencyResolver.AutofacModules
{
    public class CommandHandlerModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(CreateCommand).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<,>));
        }
    }
}

