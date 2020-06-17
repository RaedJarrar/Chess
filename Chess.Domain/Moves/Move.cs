// <copyright file="Move.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.Domain
{
    /// <summary>
    /// Defines a piece move.
    /// </summary>
    public class Move : ValueObject<Move>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Move"/> class.
        /// </summary>
        /// <param name="piece">The piece that moved.</param>
        /// <param name="from">The from cell.</param>
        /// <param name="kind">Kind of movement.</param>
        /// <param name="position">The new position of the piece.</param>
        /// <param name="isCheck">Is enemy king in check due to the move.</param>
        /// <param name="isCheckMate">Is enemy king check mated due to the move.</param>
        /// <param name="promotedTo">Which piece the pawn was promoted to.</param>
        public Move(Piece piece, Cell from, MoveKind kind, Cell position, bool isCheck = false, bool isCheckMate = false, PieceKind? promotedTo = null)
        {
            this.Piece = piece;
            this.From = from;
            this.Kind = kind;
            this.Position = position;

            this.IsCheck = isCheck;
            this.IsCheckMate = this.IsCheck && isCheckMate;
            this.PromotedTo = promotedTo;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Move"/> class.
        /// </summary>
        /// <param name="move">The move to copy from.</param>
        /// <param name="isCheck">Is enemy king in check due to the move.</param>
        /// <param name="isCheckMate">Is enemy king check mated due to the move.</param>
        public Move(Move move, bool isCheck = false, bool isCheckMate = false)
            : this(move.Piece, move.From, move.Kind, move.Position, isCheck, isCheckMate, move.PromotedTo)
        {
        }

        /// <summary>
        /// Gets the kind of the move.
        /// </summary>
        public MoveKind Kind { get; }

        /// <summary>
        /// Gets the piece that was moved.
        /// </summary>
        public Piece Piece { get; }

        /// <summary>
        /// Gets the original position of the piece prior to the move.
        /// </summary>
        public Cell From { get; }

        /// <summary>
        /// Gets the piece position after the move.
        /// </summary>
        public Cell Position { get; }

        /// <summary>
        /// Gets a value indicating whether the enemy king is in check due to the move.
        /// </summary>
        public bool IsCheck { get; }

        /// <summary>
        /// Gets a value indicating whether the enemy king is check mated due to the move.
        /// </summary>
        public bool IsCheckMate { get; }

        /// <summary>
        /// Gets the piece to which the pawn was promoted if any.
        /// </summary>
        public PieceKind? PromotedTo { get; }

        /// <inheritdoc/>
        public override string ToString()
        {
            string pieceSymbol = this.Piece.GetType() == typeof(Pawn) ? this.From.File.Name : this.Piece.Symbol;
            string checkSymbol = this.IsCheckMate ? "#" : this.IsCheck ? "+" : string.Empty;
            string promotionSymbol = this.PromotedTo.HasValue ? $"={this.GetPieceSymbolFor(this.PromotedTo.Value)}" : string.Empty;

            switch (this.Kind)
            {
                case MoveKind.Move:
                    return $"{pieceSymbol}{this.Position}{promotionSymbol}{checkSymbol}";
                case MoveKind.Take:
                    return $"{pieceSymbol}x{this.Position}{promotionSymbol}{checkSymbol}";
                case MoveKind.KingSideCastle:
                    return $"o-o{checkSymbol}";
                case MoveKind.QueenSideCastle:
                    return $"o-o-o{checkSymbol}";
                case MoveKind.Enpassant:
                    return $"{pieceSymbol}x{this.Position}.e.p{checkSymbol}";
            }

            return string.Empty;
        }

        /// <inheritdoc/>
        protected override bool EqualsCore(Move other)
        {
            return this.Kind == other.Kind && this.Piece == other.Piece && this.Position == other.Position && this.From == other.From;
        }

        /// <inheritdoc/>
        protected override int GetHashCodeCore()
        {
            return $"{this.Piece.Id}-{this.Kind}-{this.From}-{this.Position}".GetHashCode();
        }

        private string GetPieceSymbolFor(PieceKind pieceKind)
        {
            // TODO: re-factor this
            switch (pieceKind)
            {
                case PieceKind.Bishop:
                    return "B";
                case PieceKind.Knight:
                    return "N";
                case PieceKind.Rook:
                    return "R";
                case PieceKind.Queen:
                    return "Q";
                case PieceKind.King:
                    return "K";
            }

            return string.Empty;
        }
    }
}
