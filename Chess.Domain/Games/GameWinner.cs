// <copyright file="GameWinner.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.Domain
{
    /// <summary>
    /// Enumerates game winner possibilities.
    /// </summary>
    public enum GameWinner
    {
        /// <summary>
        /// White player has won.
        /// </summary>
        White,

        /// <summary>
        /// Black player has won.
        /// </summary>
        Black,

        /// <summary>
        /// The game was drawn.
        /// </summary>
        None,
    }
}
