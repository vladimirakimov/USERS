using ITG.Brix.Users.Domain.Bases;
using ITG.Brix.Users.Domain.Internal;
using System.Collections.Generic;

namespace ITG.Brix.Users.Domain
{
    public class Login : ValueObject
    {
        public string Value { get; private set; }

        public Login(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw Error.ArgumentNull(string.Format("{0} can't be empty.", nameof(value)));
            }

            Value = value;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
