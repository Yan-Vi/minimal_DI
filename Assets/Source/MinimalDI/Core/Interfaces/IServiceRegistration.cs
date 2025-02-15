namespace MinimalDI.Core.Interfaces
{
    public interface IServiceRegistration
    {
        public void RegisterService(object service);
        public void RegisterService<T, TInterface>(T service) where T : TInterface;
        public void RegisterInterfaces(object service);
    }
}