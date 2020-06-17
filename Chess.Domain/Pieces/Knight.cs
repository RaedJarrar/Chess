// <copyright file="Knight.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.Domain
{
    /// <summary>
    /// Defines a knight.
    /// </summary>
    public sealed class Knight : Piece
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Knight"/> class.
        /// </summary>
        /// <param name="gameState">The game state.</param>
        /// <param name="initialPosition">Initial position of the Knight.</param>
        /// <param name="status">The piece status.</param>
        /// <param name="id">The piece Id.</param>
        public Knight(
            IGameState gameState,
            Cell initialPosition,
            PieceStatus status = PieceStatus.InPlay,
            long? id = null)
            : base(PieceKind.Knight, gameState, initialPosition, 3, "N", new LShapePatternMovementStrategy(gameState.ChessBoard), status, id)
        {
        }
    }
}
