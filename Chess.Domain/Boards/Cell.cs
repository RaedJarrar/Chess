// <copyright file="Cell.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.Domain
{
    using System;

    /// <summary>
    /// Defines a single cell in a chess board. A cell is located using a rank and a file. For example: A1, G7, ...
    /// </summary>
    public class Cell : ValueObject<Cell>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class.
        /// </summary>
        /// <param name="file">The cell's file.</param>
        /// <param name="rank">The cell's rank.</param>
        public Cell(File file, Rank rank)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (rank == null)
            {
                throw new ArgumentNullException(nameof(rank));
            }

            this.File = file;
            this.Rank = rank;
        }

        /// <summary>
        /// Gets the cell's file.
        /// </summary>
        public File File { get; }

        /// <summary>
        /// Gets the cell's rank.
        /// </summary>
        public Rank Rank { get; }

        /// <summary>
        /// Gets a value indicating whether the cell is currently occupied by a piece.
        /// </summary>
        public bool IsOccupied => this.OccupyingPiece != null;

        /// <summary>
        /// Gets the piece that is currently in this cell if any.
        /// </summary>
        public Piece OccupyingPiece { get; private set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{this.File.Name}{this.Rank.Name}";
        }

        /// <summary>
        /// Occupy the cell using the given piece.
        /// </summary>
        /// <param name="piece">The occupying piece.</param>
        internal void Occupy(Piece piece)
        {
            if (piece == null)
            {
                throw new ArgumentNullException(nameof(piece));
            }

            if (this.IsOccupied)
            {
                throw new InvalidOperationException($"Cell: {this} is already occupied");
            }

            this.OccupyingPiece = piece;
        }

        /// <summary>
        /// Vacates the cell.
        /// </summary>
        internal void Vacate()
        {
            this.OccupyingPiece = null;
        }

        /// <inheritdoc/>
        protected override int GetHashCodeCore()
        {
            return $"{this.File.Index}-{this.Rank.Index}".GetHashCode();
        }

        /// <inheritdoc/>
        protected override bool EqualsCore(Cell other)
        {
            return this.File == other.File && this.Rank == other.Rank;
        }
    }
}
