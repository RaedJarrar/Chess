// <copyright file="KingMovementStrategy.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.Domain
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Defines the king's movement.
    /// </summary>
    internal class KingMovementStrategy : MovementStrategy, IMovementStrategy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KingMovementStrategy"/> class.
        /// </summary>
        /// <param name="chessBoard">A chess board.</param>
        public KingMovementStrategy(ChessBoard chessBoard)
            : base(chessBoard)
        {
        }

        /// <inheritdoc/>
        public IEnumerable<Move> CalculateValidMoves(Piece piece)
        {
            List<Move> validMoves = new List<Move>();

            File minFile = piece.Position.File - 1 == null ? piece.Position.File : piece.Position.File - 1;
            File maxFile = piece.Position.File + 1 == null ? piece.Position.File : piece.Position.File + 1;

            Rank minRank = piece.Position.Rank - 1 == null ? piece.Position.Rank : piece.Position.Rank - 1;
            Rank maxRank = piece.Position.Rank + 1 == null ? piece.Position.Rank : piece.Position.Rank + 1;

            for (File currentFile = minFile; currentFile != null && currentFile <= maxFile; ++currentFile)
            {
                for (Rank currentRank = minRank; currentRank != null && currentRank <= maxRank; ++currentRank)
                {
                    if (this.CanMove(piece, currentFile, currentRank))
                    {
                        validMoves.Add(this.GenerateMove(piece, currentFile, currentRank));
                    }
                }
            }

            if (!piece.HasBeenMoved && !piece.Army.IsInCheck())
            {
                IEnumerable<Piece> rooks = piece.Army.Pieces.Where(p => p.GetType() == typeof(Rook) && !p.HasBeenMoved);
                Army enemy = piece.GameState.EnemyArmy;

                foreach (Piece rook in rooks)
                {
                    bool isCastlingEligible = true;

                    if (rook.Position.File == File.H)
                    {
                        // king side castle
                        for (File file = piece.Position.File + 1; file < rook.Position.File; file++)
                        {
                            if (this.ChessBoard[file, piece.Position.Rank].IsOccupied ||
                                enemy.Pieces.Any(p => p.DoesCheck(this.ChessBoard[file, piece.Position.Rank])))
                            {
                                isCastlingEligible = false;
                                break;
                            }
                        }

                        if (isCastlingEligible)
                        {
                            validMoves.Add(new Move(piece, piece.Position, MoveKind.KingSideCastle, new Cell(File.G, piece.Position.Rank)));
                        }
                    }
                    else if (rook.Position.File == File.A)
                    {
                        // queen side castle
                        for (File file = piece.Position.File - 1; file > rook.Position.File; file--)
                        {
                            if (this.ChessBoard[file, piece.Position.Rank].IsOccupied)
                            {
                                isCastlingEligible = false;
                                break;
                            }

                            if (file != File.B && enemy.Pieces.Any(p => p.DoesCheck(this.ChessBoard[file, piece.Position.Rank])))
                            {
                                isCastlingEligible = false;
                                break;
                            }
                        }

                        if (isCastlingEligible)
                        {
                            validMoves.Add(new Move(piece, piece.Position, MoveKind.QueenSideCastle, new Cell(File.C, piece.Position.Rank)));
                        }
                    }
                }
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
            return this.CalculateValidMoves(piece)
                .Any(m => m.Position == targetCell);
        }
    }
}
