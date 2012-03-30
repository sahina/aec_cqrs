using System;

namespace Aec.Cqrs.Tests.Unit
{
    public sealed class ConsoleObserver : IObserver<ISystemEvent>
    {
        public void OnNext(ISystemEvent value)
        {
            RedirectToWhen.InvokeEvent(this, value);
        }

        public void OnError(Exception error) { }

        public void OnCompleted() { }

        void When(EnvelopeDispatched ed)
        {
            Console.WriteLine(ed);
        }

        void When(EnvelopeSent ed)
        {
            Console.WriteLine(ed);
        }

        void When(EnvelopeQuarantined e)
        {
            Console.WriteLine(e);
        }

        void When(CommandHandled e)
        {
            Console.WriteLine(e);
        }

        void When(EventHandled e)
        {
            Console.WriteLine(e);
        }
    }
}
