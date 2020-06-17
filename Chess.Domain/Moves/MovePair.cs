// <copyright file="MovePair.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.Domain
{
    /// <summary>
    /// Defines a pair of moves that represent a single turn.
    /// </summary>
    public class MovePair : ValueObject<MovePair>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MovePair"/> class.
        /// </summary>
        /// <param name="whiteMove">The white army move.</param>
        /// <param name="blackMove">The black army move.</param>
        public MovePair(Move whiteMove, Move blackMove)
        {
            this.WhiteMove = whiteMove;
            this.BlackMove = blackMove;
        }

        /// <summary>
        /// Gets the white army move.
        /// </summary>
        public Move WhiteMove { get; }

        /// <summary>
        /// Gets the black army move.
        /// </summary>
        public Move BlackMove { get; }

        /// <inheritdoc/>
        protected override int GetHashCodeCore()
        {
            return this.WhiteMove?.GetHashCode() ?? 10 % this.BlackMove?.GetHashCode() ?? 10;
        }

        /// <inheritdoc/>
        protected override bool EqualsCore(MovePair other)
        {
            return this.WhiteMove == other.WhiteMove && this.BlackMove == other.BlackMove;
        }
    }
}
