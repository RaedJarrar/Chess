// <copyright file="PieceCollection.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.Domain
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Defines a collection of pieces.
    /// </summary>
    public class PieceCollection : IReadOnlyCollection<Piece>
    {
        private readonly IEnumerable<Piece> pieces;

        /// <summary>
        /// Initializes a new instance of the <see cref="PieceCollection"/> class.
        /// </summary>
        /// <param name="pieces">A collection of pieces.</param>
        public PieceCollection(IEnumerable<Piece> pieces)
        {
            this.pieces = pieces;
        }

        /// <summary>
        /// Gets the number of pieces in the collection.
        /// </summary>
        public int Count => this.pieces.Count();

        /// <summary>
        /// Gets the piece at the given file and rank.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="rank">The rank.</param>
        /// <returns>The piece at the given file and rank.</returns>
        public Piece this[File file, Rank rank]
        {
            get
            {
                return this.pieces.FirstOrDefault(p => p.Status == PieceStatus.InPlay && p.Position.File == file && p.Position.Rank == rank);
            }
        }

        /// <summary>
        /// Gets the first piece at the given file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>The first piece at the given file.</returns>
        public Piece this[File file]
        {
            get
            {
                return this.pieces.FirstOrDefault(p => p.Position.File == file);
            }
        }

        /// <inheritdoc/>
        public IEnumerator<Piece> GetEnumerator()
        {
            return this.pieces.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.pieces.GetEnumerator();
        }

        /// <summary>
        /// Gets the piece at the given file and rank.
        /// </summary>
        /// <typeparam name="TPiece">The type of the piece.</typeparam>
        /// <param name="file">The file.</param>
        /// <param name="rank">The rank.</param>
        /// <returns>The piece at the given file and rank.</returns>
        public TPiece At<TPiece>(File file, Rank rank)
            where TPiece : Piece
        {
            return this.pieces.FirstOrDefault(p => typeof(TPiece) == p.GetType() && p.Position.File == file && p.Position.Rank == rank) as TPiece;
        }

        /// <summary>
        /// Gets the first piece at the given file.
        /// </summary>
        /// <typeparam name="TPiece">The type of the piece.</typeparam>
        /// <param name="file">The file.</param>
        /// <returns>The first piece at the given file.</returns>
        public TPiece At<TPiece>(File file)
            where TPiece : Piece
        {
            return this.pieces.FirstOrDefault(p => typeof(TPiece) == p.GetType() && p.Position.File == file) as TPiece;
        }
    }
}
