using Autofac;
using FluentValidation;
using ITG.Brix.Users.Application.Cqs.Commands.Validators;
using System.Reflection;

namespace ITG.Brix.Users.DependencyResolver.AutofacModules
{
    public class ValidatorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterAssemblyTypes(typeof(CreateCommandValidator).GetTypeInfo().Assembly)
                .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                .AsImplementedInterfaces();
        }
    }
}
