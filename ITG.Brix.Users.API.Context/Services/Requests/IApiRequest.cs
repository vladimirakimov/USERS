using ITG.Brix.Users.API.Context.Bases;

namespace ITG.Brix.Users.API.Context.Services.Requests
{
    /// <summary>
    /// Request validation strategy.
    /// </summary>
    public interface IApiRequest
    {
        ValidationResult Validate<T>(T request);
    }
}
