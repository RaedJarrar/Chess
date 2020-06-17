// <copyright file="GameStatus.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.Domain
{
    /// <summary>
    /// Enumerates different statuses the game could be at.
    /// </summary>
    public enum GameStatus
    {
        /// <summary>
        /// The game has not started yet.
        /// </summary>
        NotStarted,

        /// <summary>
        /// An in progress game.
        /// </summary>
        InProgress,

        /// <summary>
        /// A finished game.
        /// </summary>
        Finished,
    }
}
