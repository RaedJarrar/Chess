// <copyright file="StraightAndDiagonalMovementStrategy.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.Domain
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Defines a movement along straight lines and diagonals.
    /// </summary>
    internal class StraightAndDiagonalMovementStrategy : MovementStrategy, IMovementStrategy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StraightAndDiagonalMovementStrategy"/> class.
        /// </summary>
        /// <param name="chessBoard">A chess board.</param>
        public StraightAndDiagonalMovementStrategy(ChessBoard chessBoard)
            : base(chessBoard)
        {
        }

        /// <inheritdoc/>
        public IEnumerable<Move> CalculateValidMoves(Piece piece)
        {
            return new StraightMovementStrategy(this.ChessBoard).CalculateValidMoves(piece)
                .Union(new DiagonalMovementStrategy(this.ChessBoard).CalculateValidMoves(piece));
        }

        /// <inheritdoc/>
        public bool DoesCheckEnemyKing(Piece piece)
        {
            return new StraightMovementStrategy(this.ChessBoard).DoesCheckEnemyKing(piece) ||
                new DiagonalMovementStrategy(this.ChessBoard).DoesCheckEnemyKing(piece);
        }

        /// <inheritdoc/>
        public bool DoesCheck(Piece piece, Cell targetCell)
        {
            return new StraightMovementStrategy(this.ChessBoard).DoesCheck(piece, targetCell) ||
                new DiagonalMovementStrategy(this.ChessBoard).DoesCheck(piece, targetCell);
        }
    }
}
