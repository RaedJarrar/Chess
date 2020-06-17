// <copyright file="Entity.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.Domain
{
    /// <summary>
    /// The base entity class.
    /// </summary>
    public abstract class Entity
    {
        /// <summary>
        /// Gets or sets the Id of the entity.
        /// </summary>
        public long Id { get; protected set; }

        /// <summary>
        /// Checks if two entities are equal.
        /// </summary>
        /// <param name="first">The first entity.</param>
        /// <param name="second">The second entity.</param>
        /// <returns>If two entities are equal.</returns>
        public static bool operator ==(Entity first, Entity second)
        {
            if (ReferenceEquals(first, null) && ReferenceEquals(second, null))
            {
                return true;
            }

            if (ReferenceEquals(first, null) || ReferenceEquals(second, null))
            {
                return false;
            }

            return first.Equals(second);
        }

        /// <summary>
        /// Checks if two entities are not equal.
        /// </summary>
        /// <param name="first">The first entity.</param>
        /// <param name="second">The second entity.</param>
        /// <returns>If two entities are not equal.</returns>
        public static bool operator !=(Entity first, Entity second)
        {
            return !(first == second);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            var other = obj as Entity;

            if (ReferenceEquals(other, null))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (this.GetType() != other.GetType())
            {
                return false;
            }

            if (this.Id == 0 || other.Id == 0)
            {
                return false;
            }

            return this.Id == other.Id;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (this.GetType().ToString() + this.Id).GetHashCode();
        }
    }
}
