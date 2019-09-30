using System;

namespace ITG.Brix.Users.Application.Bases
{
    public class CustomFaultCode : IComparable
    {
        public static readonly CustomFaultCode NotFound = new CustomFaultCode("not-found");
        public static readonly CustomFaultCode InvalidQueryTop = new CustomFaultCode("invalid-query-top");
        public static readonly CustomFaultCode InvalidQuerySkip = new CustomFaultCode("invalid-query-skip");

        public string Name { get; private set; }

        private CustomFaultCode() { }

        public CustomFaultCode(string name)
        {
            Name = name;
        }

        public int CompareTo(object obj) => Name.CompareTo(((CustomFaultCode)obj).Name);

        public override bool Equals(object obj)
        {
            var otherValue = obj as CustomFaultCode;

            if (otherValue == null)
                return false;

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Name.Equals(otherValue.Name);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode() => Name.GetHashCode();
    }
}
