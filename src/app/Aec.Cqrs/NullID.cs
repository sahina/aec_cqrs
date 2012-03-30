namespace Aec.Cqrs
{
    public sealed class NullID : IIdentity
    {
        #region Implementation of IIdentity

        public string GetID()
        {
            return string.Empty;
        }

        #endregion
    }
}