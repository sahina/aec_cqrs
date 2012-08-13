using System;

namespace Aec.Cqrs
{
    public class FileJsonDocumentStrategy : DefaultJsonDocumentStrategy
    {
        public override string GetEntityLocation(Type entity, object key)
        {
            var identity = key as IIdentity;

            if (identity != null)
                return IdentityConvert.ToStream(identity) + ".txt";

            return key.ToString().ToLowerInvariant() + ".txt";
        }
    }
}