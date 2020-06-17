// <copyright file="BlackArmy.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.Domain
{
    using System.Collections.Generic;

    /// <summary>
    /// Black army implementation.
    /// </summary>
    public class BlackArmy : Army
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlackArmy"/> class.
        /// </summary>
        /// <param name="commander">The army's commander.</param>
        /// <param name="gameState">The game state.</param>
        /// <param name="pieces">Specifications of the army pieces. Passing null will create a brand new army.</param>
        /// <param name="id">The Id of the army.</param>
        public BlackArmy(Player commander, IGameState gameState, IEnumerable<IPieceSpecification> pieces = null, long? id = null)
            : base(commander, ArmyKind.Black, gameState, pieces, id)
        {
        }

        /// <inheritdoc/>
        protected override PieceCollection InitializePieces()
        {
            List<Piece> pieces = new List<Piece>()
            {
                new Rook(this.GameState, this.GameState.ChessBoard[File.A, Rank.Eighth]),
                new Knight(this.GameState, this.GameState.ChessBoard[File.B, Rank.Eighth]),
                new Bishop(this.GameState, this.GameState.ChessBoard[File.C, Rank.Eighth]),
                new Queen(this.GameState, this.GameState.ChessBoard[File.D, Rank.Eighth]),
                new King(this.GameState, this.GameState.ChessBoard[File.E, Rank.Eighth]),
                new Bishop(this.GameState, this.GameState.ChessBoard[File.F, Rank.Eighth]),
                new Knight(this.GameState, this.GameState.ChessBoard[File.G, Rank.Eighth]),
                new Rook(this.GameState, this.GameState.ChessBoard[File.H, Rank.Eighth]),
            };

            foreach (File file in File.Files)
            {
                pieces.Add(new Pawn(this.GameState, this.GameState.ChessBoard[file, Rank.Seventh]));
            }

            return new PieceCollection(pieces);
        }
    }
}
