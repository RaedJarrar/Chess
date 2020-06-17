// <copyright file="Piece.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Base chess piece implementation.
    /// </summary>
    public abstract class Piece : Entity
    {
        private readonly IMovementStrategy movementStrategy;
        private List<Move> moves = new List<Move>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Piece"/> class.
        /// </summary>
        /// <param name="kind">The piece kind.</param>
        /// <param name="gameState">The game state.</param>
        /// <param name="initialPosition">The initial position of the piece.</param>
        /// <param name="pointValue">The point value of the piece.</param>
        /// <param name="symbol">The symbol of the piece.</param>
        /// <param name="movementStrategy">The movement strategy used to determine the piece moves.</param>
        /// <param name="status">The piece status.</param>
        /// <param name="id">The piece Id.</param>
        public Piece(
            PieceKind kind,
            IGameState gameState,
            Cell initialPosition,
            int pointValue,
            string symbol,
            IMovementStrategy movementStrategy,
            PieceStatus status = PieceStatus.InPlay,
            long? id = null)
        {
            this.Kind = kind;

            if (gameState == null)
            {
                throw new ArgumentNullException(nameof(gameState));
            }

            if (initialPosition == null)
            {
                throw new ArgumentNullException(nameof(initialPosition));
            }

            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentNullException(nameof(symbol));
            }

            this.Id = id.HasValue ? id.Value : new Random().Next();

            this.GameState = gameState;
            this.Position = this.GameState.ChessBoard[initialPosition.File, initialPosition.Rank];
            this.PointValue = pointValue;
            this.Symbol = symbol;
            this.movementStrategy = movementStrategy;

            this.Status = status;

            if (this.Status == PieceStatus.InPlay)
            {
                this.Position.Occupy(this);
            }
        }

        /// <summary>
        /// Signals that the piece has been moved.
        /// </summary>
        internal event EventHandler<ArmyPieceMovedEvent> OnMoved;

        /// <summary>
        /// Gets the kind of the piece.
        /// </summary>
        public PieceKind Kind { get; }

        /// <summary>
        /// Gets the status of the piece.
        /// </summary>
        public PieceStatus Status { get; private set; }

        /// <summary>
        /// Gets the army this piece belongs to.
        /// </summary>
        public Army Army => this.GameState.MyArmy;

        /// <summary>
        /// Gets or sets the position of the piece.
        /// </summary>
        public Cell Position { get; protected set; }

        /// <summary>
        /// Gets the state of the game.
        /// </summary>
        public IGameState GameState { get; }

        /// <summary>
        /// Gets the point value of the piece.
        /// </summary>
        public int PointValue { get; }

        /// <summary>
        /// Gets the symbol of the piece.
        /// </summary>
        public string Symbol { get; }

        /// <summary>
        /// Gets the previous moves of the piece.
        /// </summary>
        public IReadOnlyCollection<Move> Moves { get => this.moves; }

        /// <summary>
        /// Gets a value indicating whether the piece has been moved or not.
        /// </summary>
        public bool HasBeenMoved { get => this.moves.Count > 0; }

        /// <summary>
        /// Gets the chessboard instance.
        /// </summary>
        protected ChessBoard ChessBoard => this.GameState.ChessBoard;

        /// <summary>
        /// Gets or sets the list of valid moves from the current position.
        /// </summary>
        protected List<Move> ValidMoves { get; set; }

        /// <summary>
        /// Determines whether a move to the given position is valid or not.
        /// </summary>
        /// <param name="position">The position to move to.</param>
        /// <returns>The move validity.</returns>
        public bool IsValidMove(Cell position)
        {
            this.RefreshValidMoves();

            return this.ValidMoves.Any(move => move.Position == position);
        }

        /// <summary>
        /// Gets all the valid moves for this piece in its current position.
        /// </summary>
        /// <returns>The valid moves for this piece in its current position.</returns>
        public IEnumerable<Move> GetValidMoves()
        {
            this.RefreshValidMoves();

            return this.ValidMoves;
        }

        /// <summary>
        /// Moves the piece to a new position on the chess board.
        /// </summary>
        /// <param name="newPosition">The new position.</param>
        public void Move(Cell newPosition)
        {
            this.PerformMoveValidation(newPosition);
            this.MoveCore(this.ChessBoard[newPosition.File, newPosition.Rank]);
            this.RecordMovement(this.ValidMoves.First(m => m.Position == newPosition));
        }

        /// <summary>
        /// Moves the piece to a new position on the chess board.
        /// </summary>
        /// <param name="file">The file to move to.</param>
        /// <param name="rank">The rank to move to.</param>
        public void Move(File file, Rank rank)
        {
            this.Move(new Cell(file, rank));
        }

        /// <summary>
        /// Determines if this piece checks the enemy king or not.
        /// </summary>
        /// <returns>If this piece checks the enemy king or not.</returns>
        public bool DoesCheckEnemyKing()
        {
            return this.movementStrategy.DoesCheckEnemyKing(this);
        }

        /// <summary>
        /// Determines if this piece checks the given cell.
        /// </summary>
        /// <param name="position">The target position to examine if it is being checked by the piece.</param>
        /// <returns>If this piece checks the given cell.</returns>
        public bool DoesCheck(Cell position)
        {
            return this.movementStrategy.DoesCheck(this, position);
        }

        /// <summary>
        /// Kills the piece.
        /// </summary>
        internal void Kill()
        {
            this.Position.Vacate();
            this.Status = PieceStatus.Dead;
        }

        /// <summary>
        /// Revives the piece.
        /// </summary>
        internal void Revive()
        {
            this.Status = PieceStatus.InPlay;
            this.Position.Occupy(this);
        }

        /// <summary>
        /// Ensures the move is valid before actually performing it.
        /// </summary>
        /// <param name="newPosition">The new position to move the piece to.</param>
        protected virtual void PerformMoveValidation(Cell newPosition)
        {
            if (newPosition == null)
            {
                throw new ArgumentNullException(nameof(newPosition));
            }

            if (this.Army.GameState.Status != GameStatus.InProgress)
            {
                throw new InvalidOperationException($"Game is not in progress: {this.Army.GameState.Status}");
            }

            if (this.Army.GameState.Turn != this.Army)
            {
                throw new InvalidOperationException($"It is not your turn");
            }

            if (this.Status == PieceStatus.Dead)
            {
                throw new InvalidOperationException($"{this} is dead and can't move");
            }

            if (this.Position == newPosition)
            {
                throw new InvalidOperationException($"You should specify another cell to move {this} to");
            }

            if (!this.IsValidMove(newPosition))
            {
                throw new InvalidOperationException($"{this} can't move to {newPosition}");
            }
        }

        /// <summary>
        /// Performs actual piece movement.
        /// </summary>
        /// <param name="newPosition">The new piece position.</param>
        protected virtual void MoveCore(Cell newPosition)
        {
            if (newPosition.IsOccupied)
            {
                if (newPosition.OccupyingPiece.Army == this.Army)
                {
                    throw new InvalidOperationException($"{this} can't take a piece from the same army on {newPosition}");
                }

                newPosition.OccupyingPiece.Kill();
            }

            this.Position.Vacate();
            newPosition.Occupy(this);
            this.Position = newPosition;
        }

        /// <summary>
        /// Adds a move to the piece's list of moves.
        /// </summary>
        /// <param name="move">The move to record.</param>
        protected void RecordMovement(Move move)
        {
            this.moves.Add(move);
            this.OnMoved?.Invoke(this, new ArmyPieceMovedEvent(this, move));
        }

        private void RefreshValidMoves()
        {
            Cell currentPosition = this.Position;

            this.ValidMoves = new List<Move>(this.movementStrategy.CalculateValidMoves(this));

            // eliminate moves leading to checking our king
            this.ValidMoves = this.ValidMoves.Where(move =>
            {
                Piece existingPiece = move.Kind == MoveKind.Enpassant
                    ? this.ChessBoard[move.Position.File, move.Position.Rank + (this.Army.Kind == ArmyKind.White ? -1 : 1)].OccupyingPiece
                    : move.Position.OccupyingPiece;

                this.MoveCore(this.ChessBoard[move.Position.File, move.Position.Rank]);

                bool isValid = !this.Army.IsInCheck();

                this.MoveCore(this.ChessBoard[currentPosition.File, currentPosition.Rank]);

                if (existingPiece != null)
                {
                    existingPiece.Revive();
                }

                return isValid;
            }).ToList();
        }
    }
}
