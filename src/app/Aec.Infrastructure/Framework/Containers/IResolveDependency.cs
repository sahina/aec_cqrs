using System;
using System.Collections.Generic;

namespace Aec.Infrastructure.Framework.Containers
{
    /// <summary>
    /// Contract for dependency resolver.
    /// </summary>
    /// <remarks>
    /// When using a dependency injection framework such as Unity or Windsor,
    /// implement this interface to absract the framework.
    /// </remarks>
    public interface IResolveDependency
    {
        /// <summary>
        /// Resolves the given service (dependency).
        /// </summary>
        /// <typeparam name="T">Service type.</typeparam>
        /// <returns>Service.</returns>
        T Resolve<T>();

        /// <summary>
        /// Resolves the given service (dependency).
        /// </summary>
        /// <typeparam name="T">Service type.</typeparam>
        /// <param name="parameters">Parameters to pass to service being constructed.</param>
        /// <returns>Service.</returns>
        T Resolve<T>(params object[] parameters);

        /// <summary>
        /// Resolves a named service (dependency).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns>Service.</returns>
        T Resolve<T>(string name);

        /// <summary>
        /// Resolves a named service (dependency).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="parameters">Parameters to pass to service being constructed.</param>
        /// <returns>Service.</returns>
        T Resolve<T>(string name, params object[] parameters);

        /// <summary>
        /// Resolves a service from a given type.
        /// </summary>
        /// <param name="type">Type to resolve.</param>
        /// <returns>Service as object.</returns>
        object Resolve(Type type);

        /// <summary>
        /// Resolves all registered types for the given type in the containers and returns a collection.
        /// </summary>
        /// <typeparam name="T">Type to resolve.</typeparam>
        /// <returns>List of resolved types for the given type.</returns>
        IEnumerable<T> ResolveAll<T>();

        /// <summary>
        /// Clears the list of services (dependencies).
        /// </summary>
        void Clear();
    }
}