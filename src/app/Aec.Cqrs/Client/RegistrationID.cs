using System;
using System.Runtime.Serialization;

namespace Aec.Cqrs.Client
{
    [DataContract]
    public class RegistrationID : Identity<Guid>
    {
        public const string TAG_VALUE = "registration";

        public RegistrationID(Guid id)
        {
            Identifier = id;
        }

        public override string GetTag()
        {
            return TAG_VALUE;
        }

        public override string ToString()
        {
            return string.Format("{0}-{1}", TAG_VALUE, GetIdenfitier());
        }
    }
}