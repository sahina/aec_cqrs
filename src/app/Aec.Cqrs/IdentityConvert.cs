namespace Aec.Cqrs
{
    public static class IdentityConvert
    {
        public static string ToStream(IIdentity identity)
        {
            return identity.GetTag() + "-" + identity.GetIdenfitier();
        }
    }
}