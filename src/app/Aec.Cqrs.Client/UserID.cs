using System;
using System.Runtime.Serialization;

namespace Aec.Cqrs.Client
{
    [DataContract]
    public sealed class UserID : Identity<string>
    {
        public const string TAG_VALUE = "user";

        public UserID()
        {

        }
        public UserID(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new InvalidOperationException("Tried to assemble non-existent user");

            Identifier = id;
        }

        public override string GetTag()
        {
            return TAG_VALUE;
        }
    }
}
