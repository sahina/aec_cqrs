namespace Aec.Cqrs
{
    public sealed class NullID : IIdentity
    {
        #region Implementation of IIdentity

        public string GetIdenfitier()
        {
            return string.Empty;
        }

        #endregion
    }
}