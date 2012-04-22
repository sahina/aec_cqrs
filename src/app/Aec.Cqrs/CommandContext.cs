using System;
using System.Collections.Generic;

namespace Aec.Cqrs
{
    [Serializable]
    public class CommandContext
    {
        public Guid CommandID { get; set; }
        public string Issuer { get; set; }
        public string IssuerHost { get; set; }
        public DateTime IssueDateUtc { get; set; }
        public Dictionary<string, string> Headers { get; set; }

        public CommandContext()
        {
            Headers = new Dictionary<string, string>();
        }
    }
}
