// <copyright file="StraightMovementStrategy.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.Domain
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines a movement along straight lines.
    /// </summary>
    internal class StraightMovementStrategy : MovementStrategy, IMovementStrategy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StraightMovementStrategy"/> class.
        /// </summary>
        /// <param name="chessBoard">A chess board.</param>
        public StraightMovementStrategy(ChessBoard chessBoard)
            : base(chessBoard)
        {
        }

        /// <inheritdoc/>
        public IEnumerable<Move> CalculateValidMoves(Piece piece)
        {
            List<Move> validMoves = new List<Move>();
            File currentFile;
            Rank currentRank;

            // scan to the right
            for (currentFile = piece.Position.File + 1; currentFile != null && this.CanMove(piece, currentFile, piece.Position.Rank); ++currentFile)
            {
                validMoves.Add(this.GenerateMove(piece, currentFile, piece.Position.Rank));
            }

            // scan to the left
            for (currentFile = piece.Position.File - 1; currentFile != null && this.CanMove(piece, currentFile, piece.Position.Rank); --currentFile)
            {
                validMoves.Add(this.GenerateMove(piece, currentFile, piece.Position.Rank));
            }

            // scan forward
            for (currentRank = piece.Position.Rank + 1; currentRank != null && this.CanMove(piece, piece.Position.File, currentRank); ++currentRank)
            {
                validMoves.Add(this.GenerateMove(piece, piece.Position.File, currentRank));
            }

            // scan backward
            for (currentRank = piece.Position.Rank - 1; currentRank != null && this.CanMove(piece, piece.Position.File, currentRank); --currentRank)
            {
                validMoves.Add(this.GenerateMove(piece, piece.Position.File, currentRank));
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
            bool onSameFile = piece.Position.File.Index == targetCell.File.Index;
            bool onSameRank = piece.Position.Rank.Index == targetCell.Rank.Index;

            if (onSameFile)
            {
                int rankOffset = piece.Position.Rank.Index - targetCell.Rank.Index > 0 ? -1 : 1;

                // traverse the ranks between the piece and the target cell
                for (Rank currentRank = piece.Position.Rank + rankOffset; currentRank != targetCell.Rank; currentRank += rankOffset)
                {
                    if (this.ChessBoard[piece.Position.File, currentRank].IsOccupied)
                    {
                        return false;
                    }
                }

                // no blocking pieces
                return true;
            }

            if (onSameRank)
            {
                int fileOffset = piece.Position.File.Index - targetCell.File.Index > 0 ? -1 : 1;

                // traverse the files between the piece and the target cell
                for (File currentFile = piece.Position.File + fileOffset; currentFile != targetCell.File; currentFile += fileOffset)
                {
                    if (this.ChessBoard[currentFile, piece.Position.Rank].IsOccupied)
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
