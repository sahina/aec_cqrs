﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Aec.Cqrs
{
    public static class RedirectToWhen
    {
        static readonly MethodInfo s_internalPreserveStackTraceMethod =
            typeof(Exception).GetMethod("InternalPreserveStackTrace", BindingFlags.Instance | BindingFlags.NonPublic);

        static class Cache<T>
        {
            public static readonly IDictionary<Type, MethodInfo> Dict = typeof(T)
                .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(m => m.Name == "When")
                .Where(m => m.GetParameters().Length == 1)
                .ToDictionary(m => m.GetParameters().First().ParameterType, m => m);
        }

        public static void InvokeEvent<T>(T instance, object evnt)
        {
            MethodInfo info;
            var type = evnt.GetType();
            if (!Cache<T>.Dict.TryGetValue(type, out info))
            {
                // we don't care if state does not consume events
                // they are persisted anyway
                return;
            }
            try
            {
                info.Invoke(instance, new[] { evnt });
            }
            catch (TargetInvocationException ex)
            {
                if (null != s_internalPreserveStackTraceMethod)
                    s_internalPreserveStackTraceMethod.Invoke(ex.InnerException, new object[0]);
                throw ex.InnerException;
            }
        }

        public static void InvokeCommand<T>(T instance, object command)
        {
            MethodInfo info;
            var type = command.GetType();
            if (!Cache<T>.Dict.TryGetValue(type, out info))
            {
                var s = string.Format("Failed to locate {0}.When({1})", typeof(T).Name, type.Name);
                throw new InvalidOperationException(s);
            }
            try
            {
                info.Invoke(instance, new[] { command });
            }
            catch (TargetInvocationException ex)
            {
                if (null != s_internalPreserveStackTraceMethod)
                    s_internalPreserveStackTraceMethod.Invoke(ex.InnerException, new object[0]);
                throw ex.InnerException;
            }
        }
    }
}
