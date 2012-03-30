using System.Collections.Generic;

namespace Aec.Cqrs
{
    public class Applied
    {
        public List<IEvent<IIdentity>> Events = new List<IEvent<IIdentity>>();
        public long Version;
    }
}