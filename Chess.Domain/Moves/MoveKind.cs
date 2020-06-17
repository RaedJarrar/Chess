// <copyright file="MoveKind.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.Domain
{
    /// <summary>
    /// Enumerates different move kinds.
    /// </summary>
    public enum MoveKind
    {
        /// <summary>
        /// Move.
        /// </summary>
        Move,

        /// <summary>
        /// Take.
        /// </summary>
        Take,

        /// <summary>
        /// King side castle.
        /// </summary>
        KingSideCastle,

        /// <summary>
        /// Queen side castle.
        /// </summary>
        QueenSideCastle,

        /// <summary>
        /// Enpassant.
        /// </summary>
        Enpassant,
    }
}
