// <copyright file="ArmyPieceMovedEvent.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.Domain
{
    using System;

    /// <summary>
    /// Defines an event fired upon piece movement.
    /// </summary>
    internal class ArmyPieceMovedEvent : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArmyPieceMovedEvent"/> class.
        /// </summary>
        /// <param name="piece">That piece that was moved.</param>
        /// <param name="move">The move.</param>
        public ArmyPieceMovedEvent(Piece piece, Move move)
        {
            this.Piece = piece;
            this.Move = move;
        }

        /// <summary>
        /// Gets the piece that was moved.
        /// </summary>
        public Piece Piece { get; }

        /// <summary>
        /// Gets the move.
        /// </summary>
        public Move Move { get; }
    }
}
