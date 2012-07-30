using System;

namespace Aec.Cqrs
{
    [Serializable]
    public class Identity<TKey> : IIdentity, IEquatable<TKey>
    {
        public TKey Identifier { get; protected set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            var identity = obj as Identity<TKey>;

            return identity != null && identity.Identifier.Equals(Identifier);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        bool IEquatable<TKey>.Equals(TKey other)
        {
            return Equals(other);
        }

        public override string ToString()
        {
            return string.Format("{0}-{1}", GetType().Name.Replace("Identifier", ""), Identifier);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Identifier.GetHashCode() * 397);
            }
        }

        #region Implementation of IIdentity

        public string GetIdenfitier()
        {
            return Identifier.ToString();
        }

        public string GetTag()
        {
            return GetType().ToString().ToLowerInvariant();
        }

        #endregion
    }
}