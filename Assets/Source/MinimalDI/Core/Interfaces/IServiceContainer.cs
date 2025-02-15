using System;

namespace MinimalDI.Core.Interfaces
{
    public interface IServiceContainer : IServiceRegistration, IServiceProvider
    {
        public T GetService<T>() where T : class;
        public void Remove(object item);
        public void Dispose();
    }
}