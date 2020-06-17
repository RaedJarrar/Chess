// <copyright file="LShapePatternMovementStrategy.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.Domain
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Defines a movement using L shape pattern.
    /// </summary>
    internal class LShapePatternMovementStrategy : MovementStrategy, IMovementStrategy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LShapePatternMovementStrategy"/> class.
        /// </summary>
        /// <param name="chessBoard">A chess board.</param>
        public LShapePatternMovementStrategy(ChessBoard chessBoard)
            : base(chessBoard)
        {
        }

        /// <inheritdoc/>
        public IEnumerable<Move> CalculateValidMoves(Piece piece)
        {
            List<Move> validCells = new List<Move>();

            (File file, Rank rank)[] targetCells = new (File file, Rank rank)[]
            {
                (piece.Position.File - 2, piece.Position.Rank + 1),
                (piece.Position.File - 2, piece.Position.Rank - 1),
                (piece.Position.File - 1, piece.Position.Rank + 2),
                (piece.Position.File - 1, piece.Position.Rank - 2),
                (piece.Position.File + 1, piece.Position.Rank + 2),
                (piece.Position.File + 1, piece.Position.Rank - 2),
                (piece.Position.File + 2, piece.Position.Rank + 1),
                (piece.Position.File + 2, piece.Position.Rank - 1),
            };

            return targetCells
                .Where(c => c.file != null && c.rank != null && this.CanMove(piece, c.file, c.rank))
                .Select(c => this.GenerateMove(piece, c.file, c.rank));
        }

        /// <inheritdoc/>
        public bool DoesCheckEnemyKing(Piece piece)
        {
            return this.DoesCheck(piece, piece.GameState.EnemyArmy.King.Position);
        }

        /// <inheritdoc/>
        public bool DoesCheck(Piece piece, Cell targetCell)
        {
            return this.CalculateValidMoves(piece)
                .Any(m => m.Position == targetCell);
        }
    }
}
