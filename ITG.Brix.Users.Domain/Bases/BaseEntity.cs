using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ITG.Brix.Users.Domain.Bases
{
    public abstract class BaseEntity
    {
        private const int HashMultiplier = 37;

        private Dictionary<Type, IEnumerable<PropertyInfo>> _signaturePropertiesDictionary;

        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity;
            if (ReferenceEquals(this, compareTo))
            {
                return true;
            }

            return compareTo != null && GetType().Equals(compareTo.GetType()) &&
                   HasSameSignatureAs(compareTo);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var signatureProperties = GetSignatureProperties();

                var hashCode = GetType().GetHashCode();

                hashCode = signatureProperties.Select(property => property.GetValue(this, null))
                                              .Where(value => value != null)
                                              .Aggregate(hashCode,
                                                  (current, value) =>
                                                      (current * HashMultiplier) ^ value.GetHashCode());

                return hashCode;
            }
        }

        public IEnumerable<PropertyInfo> GetSignatureProperties()
        {
            IEnumerable<PropertyInfo> properties;

            if (_signaturePropertiesDictionary == null)
            {
                _signaturePropertiesDictionary = new Dictionary<Type, IEnumerable<PropertyInfo>>();
            }

            if (_signaturePropertiesDictionary.TryGetValue(GetType(), out properties))
            {
                return properties;
            }

            return _signaturePropertiesDictionary[GetType()] = GetTypeSignatureProperties();
        }

        public bool HasSameSignatureAs(Entity compareTo)
        {
            var signatureProperties = GetSignatureProperties();

            if ((from property in signatureProperties
                 let valueOfThisObject = property.GetValue(this, null)
                 let valueToCompareTo = property.GetValue(compareTo, null)
                 where valueOfThisObject != null || valueToCompareTo != null
                 where
                 (valueOfThisObject == null) ^ (valueToCompareTo == null) ||
                 !valueOfThisObject.Equals(valueToCompareTo)
                 select valueOfThisObject).Any())
            {
                return false;
            }

            return signatureProperties.Any() || base.Equals(compareTo);
        }

        private IEnumerable<PropertyInfo> GetTypeSignatureProperties()
        {
            var result = new List<PropertyInfo>();
            var properties = GetType().GetRuntimeProperties();
            foreach (var property in properties)
            {
                foreach (var customAttribute in property.CustomAttributes)
                {
                    var type = customAttribute.AttributeType;
                    if (type == typeof(SignatureAttribute))
                    {
                        result.Add(property);
                        break;
                    }
                }
            }
            return result;
        }

    }
}
