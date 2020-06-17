// <copyright file="PieceStatus.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.Domain
{
    /// <summary>
    /// Enumerates piece statuses.
    /// </summary>
    public enum PieceStatus
    {
        /// <summary>
        /// The piece is still in the game.
        /// </summary>
        InPlay,

        /// <summary>
        /// The piece has been killed.
        /// </summary>
        Dead,
    }
}
