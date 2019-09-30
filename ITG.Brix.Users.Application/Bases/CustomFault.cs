using ITG.Brix.Users.Application.Enums;

namespace ITG.Brix.Users.Application.Bases
{
    public class CustomFault : Failure
    {
        public ErrorType Type
        {
            get
            {
                return ErrorType.CustomError;
            }
        }
    }
}
