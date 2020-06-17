// <copyright file="PawnMovementStrategy.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Defines pawn valid movements.
    /// </summary>
    internal class PawnMovementStrategy : MovementStrategy, IMovementStrategy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PawnMovementStrategy"/> class.
        /// </summary>
        /// <param name="chessBoard">A chess board.</param>
        public PawnMovementStrategy(ChessBoard chessBoard)
            : base(chessBoard)
        {
        }

        /// <inheritdoc/>
        public IEnumerable<Move> CalculateValidMoves(Piece piece)
        {
            int movementDirection = piece.Army.Kind == ArmyKind.White ? 1 : -1;
            List<Move> validMoves = new List<Move>();

            // single step march
            if (this.CanMarch(piece, piece.Position.File, piece.Position.Rank + movementDirection))
            {
                validMoves.Add(this.GenerateMove(piece, piece.Position.File, piece.Position.Rank + movementDirection));

                // double step march
                if (!piece.HasBeenMoved && this.CanMarch(piece, piece.Position.File, piece.Position.Rank + (2 * movementDirection)))
                {
                    validMoves.Add(this.GenerateMove(piece, piece.Position.File, piece.Position.Rank + (2 * movementDirection)));
                }
            }

            // kills
            if (this.CanKill(piece, piece.Position.File + 1, piece.Position.Rank + movementDirection))
            {
                validMoves.Add(this.GenerateMove(piece, piece.Position.File + 1, piece.Position.Rank + movementDirection));
            }

            if (this.CanKill(piece, piece.Position.File - 1, piece.Position.Rank + movementDirection))
            {
                validMoves.Add(this.GenerateMove(piece, piece.Position.File - 1, piece.Position.Rank + movementDirection));
            }

            // en passants
            Move enpassantKill = this.GetEnpassantKillMove(piece);

            if (enpassantKill != null)
            {
                validMoves.Add(enpassantKill);
            }

            return validMoves;
        }

        /// <inheritdoc/>
        public bool DoesCheckEnemyKing(Piece piece)
        {
            return this.DoesCheck(piece, piece.GameState.EnemyArmy.King.Position);
        }

        /// <inheritdoc/>
        public bool DoesCheck(Piece piece, Cell targetCell)
        {
            int movementDirection = piece.Army.Kind == ArmyKind.White ? 1 : -1;

            File leftFile = piece.Position.File - 1;
            File rightFile = piece.Position.File + 1;
            Rank forwardRank = piece.Position.Rank + movementDirection;

            if (leftFile != null && forwardRank != null && new Cell(leftFile, forwardRank) == targetCell)
            {
                return true;
            }

            if (rightFile != null && forwardRank != null && new Cell(rightFile, forwardRank) == targetCell)
            {
                return true;
            }

            return false;
        }

        private Move GetEnpassantKillMove(Piece piece)
        {
            Rank validEnpassantRank = piece.Army.Kind == ArmyKind.White ? Rank.Fifth : Rank.Fourth;

            if (piece.Position.Rank == validEnpassantRank)
            {
                Move lastMove = piece.Army.Kind == ArmyKind.White
                    ? piece.Army.GameState.Moves.LastOrDefault()?.BlackMove
                    : piece.Army.GameState.Moves.LastOrDefault()?.WhiteMove;

                if (lastMove != null && lastMove.Piece is Pawn pawn)
                {
                    bool isDoubleStepMarch = Math.Abs(
                        lastMove.Position.Rank.Index - lastMove.From.Rank.Index) == 2 &&
                        lastMove.From.File == lastMove.Position.File;

                    if (isDoubleStepMarch)
                    {
                        bool neighboringPawn = Math.Abs(lastMove.Position.File.Index - piece.Position.File.Index) == 1;

                        if (neighboringPawn)
                        {
                            int movementDirection = piece.Army.Kind == ArmyKind.White ? 1 : -1;
                            return new Move(
                                piece,
                                piece.Position,
                                MoveKind.Enpassant,
                                new Cell(lastMove.Position.File, piece.Position.Rank + movementDirection));
                        }
                    }
                }
            }

            return null;
        }

        private bool CanKill(Piece piece, File file, Rank rank)
        {
            if (file == null || rank == null)
            {
                return false;
            }

            Cell cell = this.ChessBoard[file, rank];

            return cell != null && cell.IsOccupied && cell.OccupyingPiece.Army != piece.Army;
        }

        private bool CanMarch(Piece piece, File file, Rank rank)
        {
            if (file == null || rank == null)
            {
                return false;
            }

            Cell cell = this.ChessBoard[file, rank];

            return !cell?.IsOccupied ?? false;
        }
    }
}
