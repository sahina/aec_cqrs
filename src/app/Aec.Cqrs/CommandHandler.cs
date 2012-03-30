using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Aec.Cqrs
{
    public sealed class CommandHandler
    {
        public readonly IDictionary<Type, Action<object>> Dict = new Dictionary<Type, Action<object>>();

        static readonly MethodInfo s_internalPreserveStackTraceMethod =
            typeof(Exception).GetMethod("InternalPreserveStackTrace", BindingFlags.Instance | BindingFlags.NonPublic);


        public void WireToWhen(object o)
        {
            var infos = o.GetType()
                .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(m => m.Name == "When")
                .Where(m => m.GetParameters().Length == 1);

            foreach (var methodInfo in infos)
            {
                var type = methodInfo.GetParameters().First().ParameterType;

                var info = methodInfo;
                Dict.Add(type, message => info.Invoke(o, new[] { message }));
            }
        }

        public void WireToLambda<T>(Action<T> handler)
        {
            Dict.Add(typeof(T), o => handler((T)o));
        }

        public void Handle(object message)
        {
            Action<object> handler;
            var type = message.GetType();

            if (!Dict.TryGetValue(type, out handler))
            {
                //Trace.WriteLine(string.Format("Discarding {0} - failed to locate event handler", type.Name));
            }
            try
            {
                handler(message);
            }
            catch (TargetInvocationException ex)
            {
                if (null != s_internalPreserveStackTraceMethod)
                    s_internalPreserveStackTraceMethod.Invoke(ex.InnerException, new object[0]);
                throw ex.InnerException;
            }
        }

        public void HandleAll(ImmutableEnvelope envelope)
        {
            foreach (var message in envelope.Items)
                Handle(message.Content);
        }
    }
}