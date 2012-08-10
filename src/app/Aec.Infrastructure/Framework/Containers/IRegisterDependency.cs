namespace Aec.Infrastructure.Framework.Containers
{
    /// <summary>
    /// Contract for registering services to dependency resolution.
    /// </summary>
    /// <remarks>
    /// When using a dependency injection framework such as Unity or Windsor,
    /// implement this interface to absract the framework.
    /// </remarks>
    public interface IRegisterDependency
    {
        /// <summary>
        /// Registers the service.
        /// </summary>
        /// <typeparam name="T">Service type.</typeparam>
        /// <param name="instance">Service.</param>
        void Register<T>(T instance);
    }
}
