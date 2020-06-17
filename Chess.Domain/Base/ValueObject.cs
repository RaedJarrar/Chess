// <copyright file="ValueObject.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.Domain
{
    /// <summary>
    /// The base value object class.
    /// </summary>
    /// <typeparam name="T">The type of the value object.</typeparam>
    public abstract class ValueObject<T>
        where T : ValueObject<T>
    {
        /// <summary>
        /// Checks if two values are equal.
        /// </summary>
        /// <param name="a">The first values.</param>
        /// <param name="b">The second values.</param>
        /// <returns>If two values are equal.</returns>
        public static bool operator ==(ValueObject<T> a, ValueObject<T> b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            {
                return true;
            }

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            {
                return false;
            }

            return a.Equals(b);
        }

        /// <summary>
        /// Checks if two values are not equal.
        /// </summary>
        /// <param name="a">The first values.</param>
        /// <param name="b">The second values.</param>
        /// <returns>If two values are not equal.</returns>
        public static bool operator !=(ValueObject<T> a, ValueObject<T> b)
        {
            return !(a == b);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            var valueObject = obj as T;

            if (ReferenceEquals(valueObject, null))
            {
                return false;
            }

            return this.EqualsCore(valueObject);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return this.GetHashCodeCore();
        }

        /// <summary>
        /// Gets the hash code of the value object.
        /// </summary>
        /// <returns>The hash code of the value object.</returns>
        protected abstract int GetHashCodeCore();

        /// <summary>
        /// Determines if the given value object is equal to this.
        /// </summary>
        /// <param name="other">The other value object.</param>
        /// <returns>If the given value object is equal to this.</returns>
        protected abstract bool EqualsCore(T other);
    }
}
