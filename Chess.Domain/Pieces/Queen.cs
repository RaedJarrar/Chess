// <copyright file="Queen.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.Domain
{
    /// <summary>
    /// Defines a Queen.
    /// </summary>
    public sealed class Queen : Piece
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Queen"/> class.
        /// </summary>
        /// <param name="gameState">The game state.</param>
        /// <param name="initialPosition">Initial position of the Queen.</param>
        /// <param name="status">The piece status.</param>
        /// <param name="id">The piece Id.</param>
        public Queen(
            IGameState gameState,
            Cell initialPosition,
            PieceStatus status = PieceStatus.InPlay,
            long? id = null)
            : base(PieceKind.Queen, gameState, initialPosition, 9, "Q", new StraightAndDiagonalMovementStrategy(gameState.ChessBoard), status, id)
        {
        }
    }
}
