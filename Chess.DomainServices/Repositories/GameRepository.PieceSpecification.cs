// <copyright file="GameRepository.PieceSpecification.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.DomainServices
{
    using Chess.Domain;

    /// <summary>
    /// Manages game persistence.
    /// </summary>
    public partial class GameRepository
    {
        private class PieceSpecification : IPieceSpecification
        {
            public PieceKind Kind { get; set; }

            public long Id { get; set; }

            public Cell Position { get; set; }

            public PieceStatus Status { get; set; }
        }
    }
}
