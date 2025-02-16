using System;
using System.Collections.Generic;

namespace MinimalDI.Core.Interfaces
{
    public interface IServiceContainer : IServiceRegistration, IServiceProvider
    {
        public IEnumerable<object> Services { get; }
        public T GetService<T>() where T : class;
        public void Remove(object item);
    }
}