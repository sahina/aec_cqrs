using System.Runtime.Serialization;

namespace Aec.Cqrs.Client.Events
{
    [DataContract]
    public class RegistrationFailed : RegistrationEvent
    {
        [DataMember(Order = 1)]
        public string[] Problems { get; private set; }

        public RegistrationFailed(RegistrationID identity, string[] problems)
            : base(identity)
        {
            Problems = problems;
        }
    }
}