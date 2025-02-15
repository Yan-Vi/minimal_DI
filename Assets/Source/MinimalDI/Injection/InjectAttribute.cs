using System;
using System.Linq;
using System.Reflection;
using UnityEngine.Scripting;

namespace MinimalDI.Injection
{
    [Preserve]
    [AttributeUsage(
        AttributeTargets.Field 
        | AttributeTargets.Property
        | AttributeTargets.Constructor 
        | AttributeTargets.Method)]
    public class InjectAttribute : Attribute
    {
        public static bool IsPresentOnObject(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            return obj.GetType()
                .GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Any(member => member.GetCustomAttributes(typeof(InjectAttribute), inherit: true).Any());
        }
    }
}