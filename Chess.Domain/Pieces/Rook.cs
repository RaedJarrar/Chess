// <copyright file="Rook.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.Domain
{
    using System;

    /// <summary>
    /// Defines a Rook.
    /// </summary>
    public sealed class Rook : Piece
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Rook"/> class.
        /// </summary>
        /// <param name="gameState">The game state.</param>
        /// <param name="initialPosition">The initial position of the piece.</param>
        /// <param name="status">The piece status.</param>
        /// <param name="id">The piece Id.</param>
        public Rook(
            IGameState gameState,
            Cell initialPosition,
            PieceStatus status = PieceStatus.InPlay,
            long? id = null)
            : base(PieceKind.Rook, gameState, initialPosition, 5, "R", new StraightMovementStrategy(gameState.ChessBoard), status, id)
        {
        }

        /// <summary>
        /// Castles the rook.
        /// </summary>
        internal void Castle()
        {
            Cell newPosition = this.ChessBoard[this.Position.File == File.H ? File.F : File.D, this.Position.Rank];
            this.Position.Vacate();
            newPosition.Occupy(this);
            this.Position = newPosition;
        }
    }
}
