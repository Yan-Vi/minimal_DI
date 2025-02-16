namespace MinimalDI.Core.Interfaces
{
    public interface IServiceRegistration
    {
        public void RegisterService(object service);
        public void RegisterService<T>(T service) where T: class;
        public void RegisterInterfaces(object service);
    }
}