using System;
using System.Diagnostics;

namespace Aec.Cqrs
{
    public static class SystemObserver
    {
        private static IObserver<ISystemEvent>[] s_observers = new IObserver<ISystemEvent>[0];

        /// <summary>
        /// Swaps the given observers with the current ones and returns the current observers.
        /// There are no observers by default.
        /// </summary>
        /// <param name="observers">New observers to observers with current ones.</param>
        /// <returns>Old observers.</returns>
        public static IObserver<ISystemEvent>[] Setup(params IObserver<ISystemEvent>[] observers)
        {
            var old = s_observers;

            s_observers = observers;

            return old;
        }

        public static void Notify(ISystemEvent @event)
        {
            foreach (var observer in s_observers)
            {
                try
                {
                    observer.OnNext(@event);
                }
                catch (Exception ex)
                {
                    var message = string.Format("Observer {0} failed with {1}", observer, ex);
                    Trace.WriteLine(message);
                }
            }
        }

        public static void Complete()
        {
            foreach (var observer in s_observers)
            {
                observer.OnCompleted();
            }
        }
    }
}