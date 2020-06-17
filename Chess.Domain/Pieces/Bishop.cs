// <copyright file="Bishop.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.Domain
{
    /// <summary>
    /// Defines a Bishop.
    /// </summary>
    public sealed class Bishop : Piece
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Bishop"/> class.
        /// </summary>
        /// <param name="gameState">The game state.</param>
        /// <param name="initialPosition">The initial position of the piece.</param>
        /// <param name="status">The piece status.</param>
        /// <param name="id">The piece Id.</param>
        public Bishop(
            IGameState gameState,
            Cell initialPosition,
            PieceStatus status = PieceStatus.InPlay,
            long? id = null)
            : base(PieceKind.Bishop, gameState, initialPosition, 3, "B", new DiagonalMovementStrategy(gameState.ChessBoard), status, id)
        {
        }
    }
}
