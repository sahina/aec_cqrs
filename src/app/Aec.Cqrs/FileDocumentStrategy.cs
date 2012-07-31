using System;

namespace Aec.Cqrs
{
    public class FileDocumentStrategy : DefaultDocumentStrategy
    {
        public override string GetEntityLocation(Type entity, object key)
        {
            if (key is IIdentity)
                return IdentityConvert.ToStream((IIdentity)key) + ".txt";

            return key.ToString().ToLowerInvariant() + ".txt";
        }
    }
}