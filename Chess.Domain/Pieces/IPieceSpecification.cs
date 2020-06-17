// <copyright file="IPieceSpecification.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.Domain
{
    /// <summary>
    /// A piece specification.
    /// </summary>
    public interface IPieceSpecification
    {
        /// <summary>
        /// Gets the kind of the piece.
        /// </summary>
        PieceKind Kind { get; }

        /// <summary>
        /// Gets the Id of the piece.
        /// </summary>
        long Id { get; }

        /// <summary>
        /// Gets the position of the piece.
        /// </summary>
        Cell Position { get; }

        /// <summary>
        /// Gets the status of the piece.
        /// </summary>
        PieceStatus Status { get; }
    }
}
