// <copyright file="ChessBoard.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.Domain
{
    using System;

    /// <summary>
    /// Defines a chess board.
    /// </summary>
    public sealed class ChessBoard : Entity
    {
        private Cell[,] board = new Cell[8, 8];

        /// <summary>
        /// Initializes a new instance of the <see cref="ChessBoard"/> class.
        /// </summary>
        public ChessBoard()
        {
            this.Id = new Random().Next();
            this.InitializeBoard();
        }

        /// <summary>
        /// Gets a cell using it's X and Y coordinates. Coordinates range from 1-8.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <returns>The matching cell.</returns>
        public Cell this[int x, int y]
        {
            get
            {
                this.ValidateCoordinates(x - 1, y - 1);

                return this.board[x - 1, y - 1];
            }
        }

        /// <summary>
        /// Gets the cell at the given file and rank.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="rank">The rank.</param>
        /// <returns>The matching cell.</returns>
        public Cell this[File file, Rank rank]
        {
            get
            {
                this.ValidateCoordinates(rank.Index - 1, file.Index - 1);

                return this.board[rank.Index - 1, file.Index - 1];
            }
        }

        private void InitializeBoard()
        {
            foreach (Rank rank in Rank.Ranks)
            {
                foreach (File file in File.Files)
                {
                    this.board[rank.Index - 1, file.Index - 1] = new Cell(file, rank);
                }
            }
        }

        private void ValidateCoordinates(int rank, int file)
        {
            if (rank < 0 || rank > 7)
            {
                throw new ArgumentException($"{rank} is invalid");
            }

            if (file < 0 || file > 7)
            {
                throw new ArgumentException($"{file} is invalid");
            }
        }
    }
}
