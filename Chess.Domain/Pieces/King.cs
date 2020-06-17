// <copyright file="King.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.Domain
{
    /// <summary>
    /// Defines the king.
    /// </summary>
    public sealed class King : Piece
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="King"/> class.
        /// </summary>
        /// <param name="gameState">The game state.</param>
        /// <param name="initialPosition">Initial position of the king.</param>
        /// <param name="status">The piece status.</param>
        /// <param name="id">The piece Id.</param>
        public King(
            IGameState gameState,
            Cell initialPosition,
            PieceStatus status = PieceStatus.InPlay,
            long? id = null)
            : base(PieceKind.King, gameState, initialPosition, 0, "K", new KingMovementStrategy(gameState.ChessBoard), status, id)
        {
            this.OnMoved += (object sender, ArmyPieceMovedEvent e) =>
            {
                if (e.Move.Kind == MoveKind.KingSideCastle)
                {
                    // move the H file rook
                    this.Army.Pieces.At<Rook>(File.H).Castle();
                }
                else if (e.Move.Kind == MoveKind.QueenSideCastle)
                {
                    // move the H file rook
                    this.Army.Pieces.At<Rook>(File.A).Castle();
                }
            };
        }
    }
}
