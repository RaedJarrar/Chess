// <copyright file="Game.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Defines a chess game.
    /// </summary>
    public class Game : AggregateRoot
    {
        private List<MovePair> moves = new List<MovePair>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        public Game()
        {
            this.Id = new Random().Next();
            this.ChessBoard = new ChessBoard();
            this.CreatedAt = DateTime.Now;
        }

        /// <summary>
        /// Gets the chess board the game is played on.
        /// </summary>
        public ChessBoard ChessBoard { get; }

        /// <summary>
        /// Gets the date at which the game was created.
        /// </summary>
        public DateTime CreatedAt { get; private set; }

        /// <summary>
        /// Gets the game winner.
        /// </summary>
        public GameWinner Winner { get; private set; } = GameWinner.None;

        /// <summary>
        /// Gets the army whose turn it is.
        /// </summary>
        public Army Turn { get; private set; }

        /// <summary>
        /// Gets the status of the game.
        /// </summary>
        public GameStatus Status { get; private set; } = GameStatus.NotStarted;

        /// <summary>
        /// Gets the moves that were played so far.
        /// </summary>
        public IReadOnlyCollection<MovePair> Moves { get => this.moves; }

        /// <summary>
        /// Gets the current move number.
        /// </summary>
        public int CurrentMove { get => this.moves.Count(); }

        /// <summary>
        /// Gets the white player.
        /// </summary>
        public Player WhitePlayer { get => this.WhiteArmy?.Commander; }

        /// <summary>
        /// Gets the black player.
        /// </summary>
        public Player BlackPlayer { get => this.BlackArmy?.Commander; }

        /// <summary>
        /// Gets a value indicating whether the game is in progress.
        /// </summary>
        public bool IsInProgress { get => this.Status == GameStatus.InProgress; }

        /// <summary>
        /// Gets a value indicating whether the game has finished.
        /// </summary>
        public bool IsFinished { get => this.Status == GameStatus.Finished; }

        /// <summary>
        /// Gets the white army.
        /// </summary>
        public Army WhiteArmy { get; private set; }

        /// <summary>
        /// Gets the black army.
        /// </summary>
        public Army BlackArmy { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        /// <param name="id">The game Id.</param>
        /// <param name="createdAt">The created at time.</param>
        /// <param name="whiteArmy">The white army.</param>
        /// <param name="blackArmy">The black army.</param>
        /// <param name="turn">The turn.</param>
        /// <param name="status">The status of the game.</param>
        /// <param name="winner">The game winner.</param>
        /// <param name="moves">The moves played so far.</param>
        public void LoadGame(
            long id,
            DateTime createdAt,
            Army whiteArmy,
            Army blackArmy,
            Army turn,
            GameStatus status,
            GameWinner winner,
            IEnumerable<MovePair> moves)
        {
            if (whiteArmy == null)
            {
                throw new ArgumentNullException(nameof(whiteArmy));
            }

            if (blackArmy == null)
            {
                throw new ArgumentNullException(nameof(blackArmy));
            }

            if (whiteArmy == blackArmy)
            {
                throw new InvalidOperationException("The armies must be different");
            }

            if (whiteArmy.Kind != ArmyKind.White)
            {
                throw new ArgumentException("Invalid white army", nameof(whiteArmy));
            }

            if (blackArmy.Kind != ArmyKind.Black)
            {
                throw new ArgumentException("Invalid black army", nameof(blackArmy));
            }

            if (turn != whiteArmy && turn != blackArmy)
            {
                throw new ArgumentException("Turn should either be white's army or black's army", nameof(turn));
            }

            if (moves == null)
            {
                throw new ArgumentNullException(nameof(moves));
            }

            this.Id = id;
            this.CreatedAt = createdAt;
            this.WhiteArmy = whiteArmy;
            this.BlackArmy = blackArmy;
            this.Turn = turn;
            this.Status = status;
            this.Winner = winner;
            this.moves = new List<MovePair>(moves);

            this.RegisterArmyMovements(this.WhiteArmy);
            this.RegisterArmyMovements(this.BlackArmy);
        }

        /// <summary>
        /// Joins the game as one of the armies.
        /// </summary>
        /// <param name="player">The player joining the game.</param>
        /// <param name="armyKind">The army the player will be commanding.</param>
        /// <returns>The army the player will command.</returns>
        public Army JoinAs(Player player, ArmyKind armyKind)
        {
            Army army = armyKind == ArmyKind.White ? this.WhiteArmy : this.BlackArmy;

            if (army != null)
            {
                if (army.Commander != player)
                {
                    throw new InvalidOperationException("This army has been claimed by another player.");
                }

                return army;
            }

            if (armyKind == ArmyKind.White)
            {
                if (this.BlackArmy != null && this.BlackArmy.Commander == player)
                {
                    throw new InvalidOperationException("Player can't play against themselves.");
                }

                army = this.WhiteArmy = new WhiteArmy(player, new GameState(this, ArmyKind.White));
            }
            else
            {
                if (this.WhiteArmy != null && this.WhiteArmy.Commander == player)
                {
                    throw new InvalidOperationException("Player can't play against themselves.");
                }

                army = this.BlackArmy = new BlackArmy(player, new GameState(this, ArmyKind.Black));
            }

            this.RegisterArmyMovements(army);

            if (this.WhiteArmy != null && this.BlackArmy != null)
            {
                this.Status = GameStatus.InProgress;
                this.Turn = this.WhiteArmy;
            }

            return army;
        }

        private void RegisterArmyMovements(Army army)
        {
            army.OnPieceMoved += (object sender, ArmyPieceMovedEvent e) =>
            {
                Army playingArmy = sender as Army;

                // log the move
                if (playingArmy.Kind == ArmyKind.White)
                {
                    this.moves.Add(new MovePair(e.Move, null));
                }
                else
                {
                    MovePair currentLastMove = this.moves.Last();
                    MovePair newMove = new MovePair(this.moves.Last().WhiteMove, e.Move);
                    this.moves.Remove(currentLastMove);
                    this.moves.Add(newMove);
                }

                Console.WriteLine($"{playingArmy} moved: {e.Move}");

                if (playingArmy.GameState.EnemyArmy.IsCheckMated())
                {
                    this.Status = GameStatus.Finished;
                    this.Winner = playingArmy.Kind == ArmyKind.White ? GameWinner.White : GameWinner.Black;
                }
                else if (playingArmy.GameState.EnemyArmy.IsStaleMated())
                {
                    this.Status = GameStatus.Finished;
                    this.Winner = GameWinner.None;
                }
                else
                {
                    // switch turns
                    this.Turn = playingArmy == this.WhiteArmy ? this.BlackArmy : this.WhiteArmy;
                }
            };
        }
    }
}
