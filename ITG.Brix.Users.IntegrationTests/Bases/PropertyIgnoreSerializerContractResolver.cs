using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ITG.Brix.Users.IntegrationTests.Bases
{
    public class PropertyIgnoreSerializerContractResolver : DefaultContractResolver
    {
        private readonly Dictionary<Type, HashSet<string>> _ignores;

        public PropertyIgnoreSerializerContractResolver()
        {
            _ignores = new Dictionary<Type, HashSet<string>>();
        }

        public void IgnoreProperty(Type type, params string[] jsonPropertyNames)
        {
            if (!_ignores.ContainsKey(type))
                _ignores[type] = new HashSet<string>();

            foreach (var prop in jsonPropertyNames)
                _ignores[type].Add(prop);
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (IsIgnored(property.DeclaringType, property.PropertyName))
                property.ShouldSerialize = i => false;

            return property;
        }

        private bool IsIgnored(Type type, string jsonPropertyName)
        {
            if (!_ignores.ContainsKey(type))
                return false;

            return _ignores[type].Contains(jsonPropertyName);
        }
    }
}
