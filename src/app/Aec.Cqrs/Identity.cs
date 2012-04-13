namespace Aec.Cqrs
{
    public class Identity<TKey> : IIdentity
    {
        public TKey ID { get; protected set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            var identity = obj as Identity<TKey>;

            return identity != null && identity.ID.Equals(ID);
        }

        public override string ToString()
        {
            return string.Format("{0}-{1}", GetType().Name.Replace("ID", ""), ID);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (ID.GetHashCode() * 397);
            }
        }

        #region Implementation of IIdentity

        public string GetID()
        {
            return ID.ToString();
        }

        #endregion
    }
}