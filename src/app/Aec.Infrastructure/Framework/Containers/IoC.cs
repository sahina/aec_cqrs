using System;
using System.Collections.Generic;

namespace Aec.Infrastructure.Framework.Containers
{
    public static class IoC
    {
        public static IResolveDependency DependencyResolver { get; private set; }

        /// <summary>
        /// Initialized the container with the given factory.
        /// </summary>
        /// <param name="factory">Resolver factory.</param>
        public static void InitilizeWith(IResolveDependencyFactory factory)
        {
            DependencyResolver = factory.CreateInstance();
        }

        /// <summary>
        /// Registers the service.
        /// </summary>
        /// <typeparam name="T">Service type.</typeparam>
        /// <param name="instance">Service.</param>
        public static void Register<T>(T instance)
        {
            if (DependencyResolver is IRegisterDependency)
                ((IRegisterDependency)DependencyResolver).Register(instance);
            else
                throw new InvalidOperationException("cannot register service");
        }

        /// <summary>
        /// Resolves a named dependency
        /// </summary>
        public static T Resolve<T>(string name)
        {
            return DependencyResolver.Resolve<T>(name);
        }

        /// <summary>
        /// Resolves a named dependency with parameters.
        /// </summary>
        public static T Resolve<T>(string name, params object[] parameters)
        {
            return DependencyResolver.Resolve<T>(name, parameters);
        }

        /// <summary>
        /// Resolves a typed dependency
        /// </summary>
        public static T Resolve<T>()
        {
            return DependencyResolver.Resolve<T>();
        }

        /// <summary>
        /// Resolves a typed dependency with parameters.
        /// </summary>
        public static T Resolve<T>(params object[] parameters)
        {
            return DependencyResolver.Resolve<T>(parameters);
        }

        /// <summary>
        /// Resolves a service from a given type.
        /// </summary>
        /// <param name="type">Type to resolve.</param>
        /// <returns>Service as object.</returns>
        public static object Resolve(Type type)
        {
            return DependencyResolver.Resolve(type);
        }

        /// <summary>
        /// Resolves all registered types for the given type in the containers and returns a collection.
        /// </summary>
        /// <typeparam name="T">Type to resolve.</typeparam>
        /// <returns>List of resolved types for the given type.</returns>
        public static IEnumerable<T> ResolveAll<T>()
        {
            return DependencyResolver.ResolveAll<T>();
        }

        /// <summary>
        /// Clears the list of dependencies.
        /// </summary>
        public static void Clear()
        {
            DependencyResolver.Clear();
        }
    }
}
