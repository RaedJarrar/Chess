// <copyright file="GameState.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.Domain
{
    using System.Collections.Generic;

    /// <summary>
    /// Game state implementation class.
    /// </summary>
    public class GameState : IGameState
    {
        private readonly Game game;
        private readonly ArmyKind armyKind;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameState"/> class.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <param name="armyKind">The army this state is for.</param>
        public GameState(Game game, ArmyKind armyKind)
        {
            this.game = game;
            this.armyKind = armyKind;
        }

        /// <inheritdoc/>
        public ChessBoard ChessBoard => this.game.ChessBoard;

        /// <inheritdoc/>
        public GameWinner Winner => this.game.Winner;

        /// <inheritdoc/>
        public Army Turn => this.game.Turn;

        /// <inheritdoc/>
        public GameStatus Status => this.game.Status;

        /// <inheritdoc/>
        public IReadOnlyCollection<MovePair> Moves => this.game.Moves;

        /// <inheritdoc/>
        public Army MyArmy => this.armyKind == ArmyKind.White ? this.game.WhiteArmy : this.game.BlackArmy;

        /// <inheritdoc/>
        public Army EnemyArmy => this.armyKind == ArmyKind.White ? this.game.BlackArmy : this.game.WhiteArmy;
    }
}
