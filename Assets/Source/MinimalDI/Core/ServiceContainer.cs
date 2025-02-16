using System;
using System.Collections.Generic;
using System.Linq;
using MinimalDI.Core.Interfaces;

namespace MinimalDI.Core
{
    public class ServiceContainer: IServiceContainer
    {
        private readonly Dictionary<Type, object> _items = new();

        public T GetService<T>() where T : class => _items.GetValueOrDefault(typeof(T)) as T;
        
        public object GetService(Type t) => _items.GetValueOrDefault(t);

        public void RegisterService<T>(T service) where T : class
        {
            Register(typeof(T), service);
        }
        
        public void RegisterService(object service)
        {
            Register(service.GetType(), service);
        }

        public void RegisterInterfaces(object service)
        {
            foreach (var type in service.GetType().GetInterfaces())
                Register(type, service);
        }

        private void Register(Type type, object obj)
        {
            _items[type] = obj;
        }

        public void Remove(object item)
        {
            var objectType = item.GetType();
            var interfaceTypes = item.GetType().GetInterfaces();
            var typeNames = new List<Type> {objectType};
            typeNames.AddRange(interfaceTypes);

            var itemsToRemove = _items
                .Where(kvp => typeNames.Contains(kvp.Key) && kvp.Value.Equals(item))
                .Select(kvp => kvp.Key)
                .ToList();

            HashSet<IDisposable> disposed = new HashSet<IDisposable>();
            foreach (var key in itemsToRemove)
            {
                var service = _items.GetValueOrDefault(key);
                if (service is IDisposable disposable && !disposed.Contains(service))
                {
                    disposed.Add(disposable);
                    disposable.Dispose();
                }
                _items.Remove(key);
            }
        }

        public void Dispose()
        {
            _items.Values.OfType<IDisposable>().ToList().ForEach(x => x.Dispose());
            _items.Clear();
        }
        
    }
}