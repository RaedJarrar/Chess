// <copyright file="IGameState.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.Domain
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the state of a chess game.
    /// </summary>
    public interface IGameState
    {
        /// <summary>
        /// Gets the chess board the game is played on.
        /// </summary>
        ChessBoard ChessBoard { get; }

        /// <summary>
        /// Gets the winner of the game.
        /// </summary>
        GameWinner Winner { get; }

        /// <summary>
        /// Gets the army which is in turn to play.
        /// </summary>
        Army Turn { get; }

        /// <summary>
        /// Gets the status of the game.
        /// </summary>
        GameStatus Status { get; }

        /// <summary>
        /// Gets a list of moves played so far in the game.
        /// </summary>
        IReadOnlyCollection<MovePair> Moves { get; }

        /// <summary>
        /// Gets the army the player is playing with.
        /// </summary>
        Army MyArmy { get; }

        /// <summary>
        /// Gets the enemy army.
        /// </summary>
        Army EnemyArmy { get; }
    }
}
