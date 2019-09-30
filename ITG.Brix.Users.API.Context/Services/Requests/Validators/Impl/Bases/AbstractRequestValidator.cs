using ITG.Brix.Users.API.Context.Bases;
using System;

namespace ITG.Brix.Users.API.Context.Services.Requests.Validators.Impl.Bases
{
    public abstract class AbstractRequestValidator<T> : IRequestValidator
    {
        public Type Type => typeof(T);

        public abstract ValidationResult Validate<T>(T request);
    }
}
