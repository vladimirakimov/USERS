using System;

namespace ITG.Brix.Users.Application.Bases
{
    public class ValidationFaultCode : IComparable
    {
        public static readonly ValidationFaultCode InvalidInput = new ValidationFaultCode("invalid-input");

        public string Name { get; private set; }

        private ValidationFaultCode() { }

        public ValidationFaultCode(string name)
        {
            Name = name;
        }

        public int CompareTo(object obj) => Name.CompareTo(((ValidationFaultCode)obj).Name);

        public override bool Equals(object obj)
        {
            var otherValue = obj as ValidationFaultCode;

            if (otherValue == null)
                return false;

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Name.Equals(otherValue.Name);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode() => Name.GetHashCode();
    }
}
