using ITG.Brix.Users.API.Context.Bases;

namespace ITG.Brix.Users.API.Context.Services.Requests.Validators.Components
{
    public interface IRequestComponentValidator
    {
        ValidationResult RouteId(string id);
        ValidationResult QueryApiVersionRequired(string apiVersion);
        ValidationResult QueryApiVersion(string apiVersion);
        ValidationResult HeaderIfMatchRequired(string ifMatch);
        ValidationResult HeaderIfMatch(string ifMatch);
        ValidationResult HeaderContentTypeRequired(string contentType);
        ValidationResult HeaderContentType(string contentType);
        ValidationResult BodyPatchRequired(string patch);
        ValidationResult BodyPatch(string patch);
    }
}
