// <copyright file="Pawn.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.Domain
{
    using System;
    using System.Linq;

    /// <summary>
    /// Defines a pawn.
    /// </summary>
    public sealed class Pawn : Piece
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Pawn"/> class.
        /// </summary>
        /// <param name="gameState">The game state.</param>
        /// <param name="initialPosition">Initial position of the Pawn.</param>
        /// <param name="status">The piece status.</param>
        /// <param name="id">The piece Id.</param>
        public Pawn(
            IGameState gameState,
            Cell initialPosition,
            PieceStatus status = PieceStatus.InPlay,
            long? id = null)
            : base(PieceKind.Pawn, gameState, initialPosition, 1, "P", new PawnMovementStrategy(gameState.ChessBoard), status, id)
        {
        }

        /// <summary>
        /// Promotes the pawn to the desired piece.
        /// </summary>
        /// <param name="newPosition">The new position the pawn is moved to.</param>
        /// <param name="promoteTo">The piece to promote the pawn to.</param>
        public void Promote(Cell newPosition, PieceKind promoteTo)
        {
            newPosition = this.ChessBoard[newPosition.File, newPosition.Rank];
            base.PerformMoveValidation(newPosition);

            if (promoteTo == PieceKind.Pawn || promoteTo == PieceKind.King)
            {
                throw new InvalidOperationException($"Pawns can't be promoted to:{promoteTo}");
            }

            Rank finalRank = this.Army.Kind == ArmyKind.White ? Rank.Eighth : Rank.First;

            if (newPosition.Rank != finalRank)
            {
                throw new InvalidOperationException("Pawns must reach the last rank to be promoted.");
            }

            Cell oldPosition = this.Position;
            Army enemy = this.Army.GameState.EnemyArmy;
            bool isKill = newPosition.IsOccupied && newPosition.OccupyingPiece?.Army != this.Army;

            this.MoveCore(this.ChessBoard[newPosition.File, newPosition.Rank]);

            this.RecordMovement(new Move(
                this,
                oldPosition,
                isKill ? MoveKind.Take : MoveKind.Move,
                this.Position,
                enemy.IsInCheck(),
                enemy.IsCheckMated(),
                promoteTo));
        }

        /// <inheritdoc/>
        protected override void PerformMoveValidation(Cell newPosition)
        {
            base.PerformMoveValidation(newPosition);

            Rank finalRank = this.Army.Kind == ArmyKind.White ? Rank.Eighth : Rank.First;

            if (newPosition.Rank == finalRank)
            {
                throw new InvalidOperationException("Call the Promote method to promote pawn.");
            }
        }

        /// <inheritdoc/>
        protected override void MoveCore(Cell newPosition)
        {
            base.MoveCore(newPosition);
            Move move = this.ValidMoves?.FirstOrDefault(m => m.Position == newPosition);

            if (move?.Kind == MoveKind.Enpassant)
            {
                int movementDirection = this.Army.Kind == ArmyKind.White ? -1 : 1;
                this.ChessBoard[newPosition.File, newPosition.Rank + movementDirection].OccupyingPiece.Kill();
            }
        }
    }
}
