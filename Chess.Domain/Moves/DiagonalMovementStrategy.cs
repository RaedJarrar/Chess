// <copyright file="DiagonalMovementStrategy.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.Domain
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines a movement along diagonals.
    /// </summary>
    internal class DiagonalMovementStrategy : MovementStrategy, IMovementStrategy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DiagonalMovementStrategy"/> class.
        /// </summary>
        /// <param name="chessBoard">A chess board.</param>
        public DiagonalMovementStrategy(ChessBoard chessBoard)
            : base(chessBoard)
        {
        }

        /// <inheritdoc/>
        public IEnumerable<Move> CalculateValidMoves(Piece piece)
        {
            List<Move> validMoves = new List<Move>();
            File currentFile;
            Rank currentRank;

            for (currentFile = piece.Position.File + 1, currentRank = piece.Position.Rank + 1;
                currentFile != null && currentRank != null && this.CanMove(piece, currentFile, currentRank);
                ++currentFile, ++currentRank)
            {
                validMoves.Add(this.GenerateMove(piece, currentFile, currentRank));
            }

            // scan to the NW diagonal
            for (currentFile = piece.Position.File - 1, currentRank = piece.Position.Rank + 1;
                currentFile != null && currentRank != null && this.CanMove(piece, currentFile, currentRank);
                --currentFile, ++currentRank)
            {
                validMoves.Add(this.GenerateMove(piece, currentFile, currentRank));
            }

            // scan to the SE diagonal
            for (currentFile = piece.Position.File + 1, currentRank = piece.Position.Rank - 1;
                currentFile != null && currentRank != null && this.CanMove(piece, currentFile, currentRank);
                ++currentFile, --currentRank)
            {
                validMoves.Add(this.GenerateMove(piece, currentFile, currentRank));
            }

            // scan to the SW diagonal
            for (currentFile = piece.Position.File - 1, currentRank = piece.Position.Rank - 1;
                currentFile != null && currentRank != null && this.CanMove(piece, currentFile, currentRank);
                --currentFile, --currentRank)
            {
                validMoves.Add(this.GenerateMove(piece, currentFile, currentRank));
            }

            return validMoves;
        }

        /// <inheritdoc/>
        public bool DoesCheckEnemyKing(Piece piece)
        {
            return this.DoesCheck(piece, piece.GameState.EnemyArmy.King.Position);
        }

        /// <inheritdoc/>
        public bool DoesCheck(Piece piece, Cell targetCell)
        {
            int fileDelta = piece.Position.File.Index - targetCell.File.Index;
            int rankDelta = piece.Position.Rank.Index - targetCell.Rank.Index;

            // pieces are on the same diagonal?
            if (Math.Abs(fileDelta) == Math.Abs(rankDelta))
            {
                int fileOffset = fileDelta > 0 ? -1 : 1;
                int rankOffset = rankDelta > 0 ? -1 : 1;

                File currentFile = piece.Position.File + fileOffset;
                Rank currentRank = piece.Position.Rank + rankOffset;

                // traverse the diagonal between the piece and the target cell
                for (; currentFile != targetCell.File && currentRank != targetCell.Rank; currentFile += fileOffset, currentRank += rankOffset)
                {
                    if (this.ChessBoard[currentFile, currentRank].IsOccupied)
                    {
                        return false;
                    }
                }

                // no blocking pieces
                return true;
            }

            return false;
        }
    }
}
