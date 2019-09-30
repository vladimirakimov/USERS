using ITG.Brix.Users.Infrastructure.ClassMaps.Bases;
using System;
using System.Linq;
using System.Reflection;

namespace ITG.Brix.Users.Infrastructure.ClassMaps
{
    public static class ClassMapsRegistrator
    {
        public static void RegisterMaps()
        {
            var assembly = Assembly.GetAssembly(typeof(ClassMapsRegistrator));

            var classMaps = assembly.GetTypes()
                  .Where(t => t.BaseType != null && t.BaseType.IsGenericType &&
                    t.BaseType.GetGenericTypeDefinition() == typeof(DomainClassMap<>));

            foreach (var classMap in classMaps)
            {
                Activator.CreateInstance(classMap);
            }
        }
    }
}
