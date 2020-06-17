// <copyright file="MovementStrategy.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.Domain
{
    /// <summary>
    /// Base movement strategy implementation.
    /// </summary>
    internal abstract class MovementStrategy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MovementStrategy"/> class.
        /// </summary>
        /// <param name="chessBoard">A chess board.</param>
        public MovementStrategy(ChessBoard chessBoard)
        {
            this.ChessBoard = chessBoard;
        }

        /// <summary>
        /// Gets the chess board.
        /// </summary>
        protected ChessBoard ChessBoard { get; }

        /// <summary>
        /// Determines if a move to a position is legal or not.
        /// </summary>
        /// <param name="piece">The piece to check for.</param>
        /// <param name="file">The file to move to.</param>
        /// <param name="rank">The rank to move to.</param>
        /// <returns>If a move to a position is legal or not.</returns>
        protected bool CanMove(Piece piece, File file, Rank rank)
        {
            if (piece.Position.File == file && piece.Position.Rank == rank)
            {
                return false;
            }

            Cell cell = this.ChessBoard[file, rank];

            if (!cell.IsOccupied)
            {
                return true;
            }
            else if (cell.OccupyingPiece.Army == piece.Army)
            {
                // another piece from the same army is blocking the move
                return false;
            }
            else
            {
                // an enemy piece is in the cell, this is a take
                return true;
            }
        }

        /// <summary>
        /// Builds a move object representing moving the piece to the given position.
        /// </summary>
        /// <param name="piece">The piece to generate the move for.</param>
        /// <param name="file">The file to move to.</param>
        /// <param name="rank">The rank to move to.</param>
        /// <returns>The move.</returns>
        protected Move GenerateMove(Piece piece, File file, Rank rank)
        {
            Cell cell = this.ChessBoard[file, rank];

            if (!cell.IsOccupied)
            {
                return new Move(piece, piece.Position, MoveKind.Move, cell);
            }
            else if (cell.OccupyingPiece.Army == piece.Army)
            {
                // another piece from the same army is blocking the move
                return null;
            }
            else
            {
                // an enemy piece is in the cell, this is a take
                return new Move(piece, piece.Position, MoveKind.Take, cell);
            }
        }
    }
}
