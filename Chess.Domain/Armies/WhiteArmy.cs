// <copyright file="WhiteArmy.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.Domain
{
    using System.Collections.Generic;

    /// <summary>
    /// White army implementation.
    /// </summary>
    public class WhiteArmy : Army
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WhiteArmy"/> class.
        /// </summary>
        /// <param name="commander">The army's commander.</param>
        /// <param name="gameState">The game state.</param>
        /// <param name="pieces">Specifications of the army pieces. Passing null will create a brand new army.</param>
        /// <param name="id">The Id of the army.</param>
        public WhiteArmy(
            Player commander,
            IGameState gameState,
            IEnumerable<IPieceSpecification> pieces = null,
            long? id = null)
            : base(commander, ArmyKind.White, gameState, pieces, id)
        {
        }

        /// <inheritdoc/>
        protected override PieceCollection InitializePieces()
        {
            List<Piece> pieces = new List<Piece>()
            {
                new Rook(this.GameState, this.GameState.ChessBoard[File.A, Rank.First]),
                new Knight(this.GameState, this.GameState.ChessBoard[File.B, Rank.First]),
                new Bishop(this.GameState, this.GameState.ChessBoard[File.C, Rank.First]),
                new Queen(this.GameState, this.GameState.ChessBoard[File.D, Rank.First]),
                new King(this.GameState, this.GameState.ChessBoard[File.E, Rank.First]),
                new Bishop(this.GameState, this.GameState.ChessBoard[File.F, Rank.First]),
                new Knight(this.GameState, this.GameState.ChessBoard[File.G, Rank.First]),
                new Rook(this.GameState, this.GameState.ChessBoard[File.H, Rank.First]),
            };

            foreach (File file in File.Files)
            {
                pieces.Add(new Pawn(this.GameState, this.GameState.ChessBoard[file, Rank.Second]));
            }

            return new PieceCollection(pieces);
        }
    }
}
