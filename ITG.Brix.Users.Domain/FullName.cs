using ITG.Brix.Users.Domain.Bases;
using ITG.Brix.Users.Domain.Internal;
using System.Collections.Generic;

namespace ITG.Brix.Users.Domain
{
    public class FullName : ValueObject
    {
        public FirstName FirstName { get; private set; }
        public LastName LastName { get; private set; }

        public void ChangeFirstName(string firstName)
        {
            FirstName = new FirstName(firstName);
        }

        public void ChangeLastName(string lastName)
        {
            LastName = new LastName(lastName);
        }
        public FullName(FirstName firstName, LastName lastName)
        {
            if (object.Equals(firstName, null))
            {
                throw Error.ArgumentNull(string.Format("{0} can't be null.", nameof(firstName)));
            }
            if (object.Equals(lastName, null))
            {
                throw Error.ArgumentNull(string.Format("{0} can't be null.", nameof(lastName)));
            }

            FirstName = firstName;
            LastName = lastName;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return FirstName;
            yield return LastName;
        }
    }
}
