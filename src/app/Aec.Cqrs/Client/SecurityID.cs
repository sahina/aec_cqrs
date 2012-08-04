using System.Runtime.Serialization;

namespace Aec.Cqrs.Client
{
    [DataContract]
    public sealed class SecurityID : Identity<string>
    {
        public const string TAG_VALUE = "security";

        public SecurityID(string id)
        {
            Identifier = id;
        }

        public override string GetTag()
        {
            return TAG_VALUE;
        }
    }
}