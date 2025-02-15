using System.Collections.Generic;
using MinimalDI.Core.Interfaces;

namespace MinimalDI.Core
{
    public class MinDI
    {
        public static IServiceContainer Default
        {
            get => Contexts[nameof(Default)];
            set => Contexts[nameof(Default)] = value;
        }

        public static Dictionary<string, IServiceContainer> Contexts = new();
        
        public static void DefaultInit() => Default = new ServiceContainer();
        public static void Dispose()
        { 
            foreach (var container in Contexts.Values)
            {
                container.Dispose();
            }
            Contexts.Clear();
        }
    }
}