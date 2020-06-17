// <copyright file="IMovementStrategy.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.Domain
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines movement strategies.
    /// </summary>
    public interface IMovementStrategy
    {
        /// <summary>
        /// Gets a list of all potential cells that can be moved to from a given position.
        /// </summary>
        /// <param name="piece">The piece to be moved.</param>
        /// <returns>A list of all potential cells that can be moved to from a given position.</returns>
        IEnumerable<Move> CalculateValidMoves(Piece piece);

        /// <summary>
        /// Determines if a piece checks the enemy king or not.
        /// </summary>
        /// <param name="piece">The piece.</param>
        /// <returns>If a piece checks the enemy king or not.</returns>
        bool DoesCheckEnemyKing(Piece piece);

        /// <summary>
        /// Determines if a piece pauses a check to the given target cell.
        /// </summary>
        /// <param name="piece">The piece.</param>
        /// <param name="targetCell">The target cell to examine if it is checked by the piece.</param>
        /// <returns>If a piece pauses a check to the given target cell.</returns>
        bool DoesCheck(Piece piece, Cell targetCell);
    }
}
