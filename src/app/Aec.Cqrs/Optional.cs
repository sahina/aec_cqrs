﻿using System;

namespace Aec.Cqrs
{
    /// <summary>
    /// Helper class that indicates nullable value in a good-citizenship code and 
    /// provides some additional piping syntax
    /// </summary>
    /// <typeparam name="T">underlying type</typeparam>
    [Serializable]
    public sealed class Optional<T> : IEquatable<Optional<T>>
    {
        private readonly T m_value;
        private readonly bool m_hasValue;

        Optional(T item, bool hasValue)
        {
            m_value = item;
            m_hasValue = hasValue;
        }

        Optional(T value)
            : this(value, true)
        {
            // ReSharper disable CompareNonConstrainedGenericWithNull
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            // ReSharper restore CompareNonConstrainedGenericWithNull
        }

        /// <summary>
        /// Default empty instance.
        /// </summary>
        public static readonly Optional<T> Empty = new Optional<T>(default(T), false);

        /// <summary>
        /// Gets the underlying value.
        /// </summary>
        /// <value>The value.</value>
        public T Value
        {
            get
            {
                if (!m_hasValue)
                {
                    throw new InvalidOperationException("Dont access value when maybe is empty");
                }

                return m_value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has value.
        /// </summary>
        /// <value><c>true</c> if this instance has value; otherwise, <c>false</c>.</value>
        public bool HasValue
        {
            get { return m_hasValue; }
        }

        /// <summary>
        /// Retrieves value from this instance, using a 
        /// <paramref name="defaultValue"/> if it is absent.
        /// </summary>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>value</returns>
        public T GetValue(T defaultValue)
        {
            return m_hasValue ? m_value : defaultValue;
        }

        /// <summary>
        /// Retrieves value from this instance, using a 
        /// <paramref name="defaultValue"/> if it is absent.
        /// </summary>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>value</returns>
        public T GetValue(Func<T> defaultValue)
        {
            return m_hasValue ? m_value : defaultValue();
        }

        /// <summary>
        /// Retrieves value from this instance, using a <paramref name="defaultValue"/>
        /// factory, if it is absent
        /// </summary>
        /// <param name="defaultValue">The default value to provide.</param>
        /// <returns>maybe value</returns>
        public Optional<T> Combine(Func<Optional<T>> defaultValue)
        {
            return m_hasValue ? this : defaultValue();
        }


        /// <summary>
        /// Converts this instance to <see cref="Optional{T}"/>, 
        /// while applying <paramref name="converter"/> if there is a value.
        /// </summary>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="converter">The converter.</param>
        /// <returns></returns>
        public Optional<TTarget> Convert<TTarget>(Func<T, TTarget> converter)
        {
            return m_hasValue ? converter(m_value) : Optional<TTarget>.Empty;
        }

        /// <summary>
        /// Retrieves converted value, using a 
        /// <paramref name="defaultValue"/> if it is absent.
        /// </summary>
        /// <typeparam name="TTarget">type of the conversion target</typeparam>
        /// <param name="converter">The converter.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>value</returns>
        public TTarget Convert<TTarget>(Func<T, TTarget> converter, Func<TTarget> defaultValue)
        {
            return m_hasValue ? converter(m_value) : defaultValue();
        }

        /// <summary>
        /// Retrieves converted value, using a 
        /// <paramref name="defaultValue"/> if it is absent.
        /// </summary>
        /// <typeparam name="TTarget">type of the conversion target</typeparam>
        /// <param name="converter">The converter.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>value</returns>
        public TTarget Convert<TTarget>(Func<T, TTarget> converter, TTarget defaultValue)
        {
            return m_hasValue ? converter(m_value) : defaultValue;
        }

        /// <summary>
        /// Retrieves converted value.
        /// </summary>
        /// <typeparam name="TTarget">type of the conversion target</typeparam>
        /// <param name="converter">The converter.</param>
        /// <returns>value</returns>
        public Optional<TTarget> Combine<TTarget>(Func<T, Optional<TTarget>> converter)
        {
            return m_hasValue ? converter(m_value) : Optional<TTarget>.Empty;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Optional{T}"/> is equal to the current <see cref="Optional{T}"/>.
        /// </summary>
        /// <param name="optional">The <see cref="Optional{T}"/> to compare with.</param>
        /// <returns>true if the objects are equal</returns>
        public bool Equals(Optional<T> optional)
        {
            if (ReferenceEquals(null, optional)) return false;
            if (ReferenceEquals(this, optional)) return true;

            if (m_hasValue != optional.m_hasValue) return false;
            if (!m_hasValue) return true;
            return m_value.Equals(optional.m_value);
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            var maybe = obj as Optional<T>;
            if (maybe == null) return false;
            return Equals(maybe);
        }

        /// <summary>
        /// Serves as a hash function for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="Optional{T}"/>.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                // ReSharper disable CompareNonConstrainedGenericWithNull
                return ((m_value != null ? m_value.GetHashCode() : 0) * 397) ^ m_hasValue.GetHashCode();
                // ReSharper restore CompareNonConstrainedGenericWithNull
            }
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Optional<T> left, Optional<T> right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Optional<T> left, Optional<T> right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        /// Performs an implicit conversion from <typeparamref name="T"/> to <see cref="Optional{T}"/>.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Optional<T>(T item)
        {
            // ReSharper disable CompareNonConstrainedGenericWithNull
            if (item == null) throw new ArgumentNullException("item");
            // ReSharper restore CompareNonConstrainedGenericWithNull

            return new Optional<T>(item);
        }


        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            if (m_hasValue)
            {
                return "<" + m_value + ">";
            }

            return "<Empty>";
        }

        /// <summary>
        /// Eagerly applies the <paramref name="applicator"/> function, if the optional has a value.
        /// </summary>
        /// <param name="applicator">The applicator function.</param>
        /// <returns>same instance for further chaining</returns>
        public Optional<T> IfValue(Action<T> applicator)
        {
            if (m_hasValue)
            {
                applicator(m_value);
            }
            return this;
        }

        /// <summary>
        /// Eagerly executes the <paramref name="applicator"/> functional,
        /// if this optional does not have a value.
        /// </summary>
        /// <param name="applicator">The applicator.</param>
        /// <returns>same instance for further chaining</returns>
        public Optional<T> IfEmpty(Action applicator)
        {
            if (!m_hasValue)
            {
                applicator();
            }
            return this;
        }
    }
}
