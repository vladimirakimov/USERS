using ITG.Brix.Users.Application.Bases;
using System.Collections.Generic;

namespace ITG.Brix.Users.API.Context.Bases
{
    public class ValidationResult
    {
        public ValidationResult()
        {
            ServiceError = ServiceError.None;
        }

        public ServiceError ServiceError { get; set; }

        public IList<Failure> Errors { get; set; }

        public bool HasErrors
        {
            get
            {
                return ServiceError != ServiceError.None;
            }
        }
    }
}
