using System;
using System.Reflection;

namespace MinimalDI.Injection
{
    public static class InjectionExtensions
    {
        public static T Resolve<T>(this T obj, IServiceProvider locator = null)
        {
            locator ??= Core.MinDI.Default;
            locator.InjectFields(obj);
            locator.InjectProperties(obj);
            return obj;
        }
        
        public static T ResolveChildFields<T>(this T obj, IServiceProvider locator = null)
        {
            var fields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (var field in fields)
            {
                field.GetValue(obj)?.Resolve(locator);
            }
            return obj;
        }

        private static void InjectFields(this IServiceProvider locator, object obj)
        {
            var fields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (var field in fields)
            {
                if (!Attribute.IsDefined(field, typeof(InjectAttribute))) continue;

                var service = locator.GetService(field.FieldType);
                if (service != null)
                {
                    field.SetValue(obj, service);
                }
            }
        }

        private static void InjectProperties(this IServiceProvider locator, object obj)
        {
            var properties = obj.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (var property in properties)
            {
                if (!Attribute.IsDefined(property, typeof(InjectAttribute)) || !property.CanWrite) continue;

                var service = locator.GetService(property.PropertyType);
                if (service != null)
                {
                    property.SetValue(obj, service);
                }
            }
        }
    }
}