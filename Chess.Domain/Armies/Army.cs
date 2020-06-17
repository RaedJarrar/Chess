// <copyright file="Army.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A chess army base implementation.
    /// </summary>
    public abstract class Army : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Army"/> class.
        /// </summary>
        /// <param name="commander">The army commander.</param>
        /// <param name="kind">The kind of the army.</param>
        /// <param name="gameState">The game state.</param>
        /// <param name="pieces">Specifications of the army pieces. Passing null will create a brand new army.</param>
        /// <param name="id">The Id of the army.</param>
        public Army(Player commander, ArmyKind kind, IGameState gameState, IEnumerable<IPieceSpecification> pieces = null, long? id = null)
        {
            if (commander == null)
            {
                throw new ArgumentNullException(nameof(commander));
            }

            if (gameState == null)
            {
                throw new ArgumentNullException(nameof(gameState));
            }

            this.Id = id.HasValue ? id.Value : new Random().Next();
            this.Kind = kind;
            this.Commander = commander;
            this.GameState = gameState;
            this.Pieces = pieces != null ? this.IngestPieces(pieces) : this.InitializePieces();
            this.King = this.Pieces.First(p => p is King) as King;

            foreach (Piece piece in this.Pieces)
            {
                piece.OnMoved += this.HandlePieceMoved;
            }
        }

        /// <summary>
        /// Signals when a piece has been moved.
        /// </summary>
        internal event EventHandler<ArmyPieceMovedEvent> OnPieceMoved;

        /// <summary>
        /// Gets the army kind (White or Black).
        /// </summary>
        public ArmyKind Kind { get; }

        /// <summary>
        /// Gets the state of the game.
        /// </summary>
        public IGameState GameState { get; }

        /// <summary>
        /// Gets the pieces in the army.
        /// </summary>
        public PieceCollection Pieces { get; private set; }

        /// <summary>
        /// Gets the king.
        /// </summary>
        public King King { get; }

        /// <summary>
        /// Gets the army's commander.
        /// </summary>
        public Player Commander { get; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.Kind.ToString();
        }

        /// <summary>
        /// Checks if the king has been check mated.
        /// </summary>
        /// <returns>If the king has been check mated.</returns>
        public bool IsCheckMated()
        {
            return this.IsInCheck() && !this.Pieces.Where(p => p.Status == PieceStatus.InPlay).Any(p => p.GetValidMoves().Any());
        }

        /// <summary>
        /// Checks if the king has been stale mated.
        /// </summary>
        /// <returns>If the king has been stale mated.</returns>
        public bool IsStaleMated()
        {
            return !this.IsInCheck() && !this.Pieces.Where(p => p.Status == PieceStatus.InPlay).Any(p => p.GetValidMoves().Any());
        }

        /// <summary>
        /// Checks if the king is in check.
        /// </summary>
        /// <returns>If the king is in check.</returns>
        public bool IsInCheck()
        {
            // check if there are checks to the opposing side king
            return this.GameState.EnemyArmy.Pieces.Where(p => p.Status == PieceStatus.InPlay && !(p is King)).Any(
                p => p.DoesCheckEnemyKing());
        }

        /// <summary>
        /// Configures and places the pieces of the army.
        /// </summary>
        /// <returns>The army pieces.</returns>
        protected abstract PieceCollection InitializePieces();

        /// <summary>
        /// Builds a piece collection out of the given piece specifications.
        /// </summary>
        /// <param name="pieces">Piece specifications.</param>
        /// <returns>The army's piece collection.</returns>
        private PieceCollection IngestPieces(IEnumerable<IPieceSpecification> pieces)
        {
            return new PieceCollection(pieces.Select<IPieceSpecification, Piece>(spec =>
            {
                switch (spec.Kind)
                {
                    case PieceKind.Pawn:
                        return new Pawn(this.GameState, this.GameState.ChessBoard[spec.Position.File, spec.Position.Rank], spec.Status, spec.Id);
                    case PieceKind.Bishop:
                        return new Bishop(this.GameState, this.GameState.ChessBoard[spec.Position.File, spec.Position.Rank], spec.Status, spec.Id);
                    case PieceKind.Knight:
                        return new Knight(this.GameState, this.GameState.ChessBoard[spec.Position.File, spec.Position.Rank], spec.Status, spec.Id);
                    case PieceKind.Rook:
                        return new Rook(this.GameState, this.GameState.ChessBoard[spec.Position.File, spec.Position.Rank], spec.Status, spec.Id);
                    case PieceKind.Queen:
                        return new Queen(this.GameState, this.GameState.ChessBoard[spec.Position.File, spec.Position.Rank], spec.Status, spec.Id);
                    case PieceKind.King:
                        return new King(this.GameState, this.GameState.ChessBoard[spec.Position.File, spec.Position.Rank], spec.Status, spec.Id);
                    default:
                        throw new InvalidOperationException("Unknown piece kind");
                }
            }).ToList());
        }

        private void HandlePieceMoved(object sender, ArmyPieceMovedEvent e)
        {
            Piece movedPiece = sender as Piece;
            Army enemy = this.GameState.EnemyArmy;

            if (e.Move.PromotedTo.HasValue)
            {
                // this is a promotion, kill the pawn and replace it with the promoted to piece
                Cell position = movedPiece.Position;
                movedPiece.Kill();
                Piece promotedToPiece;

                switch (e.Move.PromotedTo.Value)
                {
                    case PieceKind.Bishop:
                        promotedToPiece = new Bishop(this.GameState, position);
                        break;
                    case PieceKind.Knight:
                        promotedToPiece = new Knight(this.GameState, position);
                        break;
                    case PieceKind.Rook:
                        promotedToPiece = new Rook(this.GameState, position);
                        break;
                    case PieceKind.Queen:
                        promotedToPiece = new Queen(this.GameState, position);
                        break;
                    default:
                        throw new InvalidOperationException("Invalid promotion");
                }

                promotedToPiece.OnMoved += this.HandlePieceMoved;
                this.Pieces = new PieceCollection(this.Pieces.Union(new Piece[] { promotedToPiece }));
            }

            this?.OnPieceMoved(this, new ArmyPieceMovedEvent(
                e.Piece,
                new Move(e.Move, enemy.IsInCheck(), enemy.IsCheckMated())));
        }
    }
}
