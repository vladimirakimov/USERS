using ITG.Brix.Users.API.Context.Bases;
using System;

namespace ITG.Brix.Users.API.Context.Services.Requests.Validators
{
    public interface IRequestValidator
    {
        ValidationResult Validate<T>(T request);

        Type Type { get; }
    }
}
