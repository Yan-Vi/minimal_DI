using System.Collections.Generic;
using System.Reflection;
using MinimalDI.Core.Interfaces;

namespace MinimalDI.Core
{
    public static class ServiceRegistrationExtensions
    {
        public static void RegisterPublicFields(this IServiceRegistration serviceRegistration, object obj)
        {
            var fields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
            foreach (var field in fields)
            {
                if (field.GetValue(obj) is not object fieldValue) continue;
                serviceRegistration.RegisterService(fieldValue);
                serviceRegistration.RegisterInterfaces(fieldValue);
            }
        }
        public static void RegisterCollectionMembers<T>(this IServiceRegistration serviceRegistration, IEnumerable<T> members) where T : class
        {
            foreach (var field in members)
            {
                if (field is not object fieldValue) continue;
                serviceRegistration.RegisterService(fieldValue);
                serviceRegistration.RegisterInterfaces(fieldValue);
            }
        }
    }
}