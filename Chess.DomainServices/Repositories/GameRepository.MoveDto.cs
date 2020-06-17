// <copyright file="GameRepository.MoveDto.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.DomainServices
{
    using System.Linq;
    using System.Runtime.Serialization;
    using Chess.Domain;

    /// <summary>
    /// Manages game persistence.
    /// </summary>
    public partial class GameRepository
    {
        [DataContract]
        private class MoveDto
        {
            public MoveDto()
            {
            }

            public MoveDto(Move move)
            {
                this.PieceId = move.Piece.Id;
                this.FromFile = move.From.File.Index;
                this.FromRank = move.From.Rank.Index;
                this.File = move.Position.File.Index;
                this.Rank = move.Position.Rank.Index;
                this.IsCheck = move.IsCheck;
                this.IsCheckMate = move.IsCheckMate;
                this.Kind = move.Kind;
                this.PromotedTo = move.PromotedTo;
            }

            [DataMember]
            public long PieceId { get; set; }

            [DataMember]
            public int FromFile { get; set; }

            [DataMember]
            public int FromRank { get; set; }

            [DataMember]
            public int File { get; set; }

            [DataMember]
            public int Rank { get; set; }

            [DataMember]
            public MoveKind Kind { get; set; }

            [DataMember]
            public bool IsCheck { get; set; }

            [DataMember]
            public bool IsCheckMate { get; set; }

            [DataMember]
            public PieceKind? PromotedTo { get; set; }

            public Move ToMove(ChessBoard board, Army army)
            {
                return new Move(
                    army.Pieces.First(p => p.Id == this.PieceId),
                    board[new File(this.FromFile), new Rank(this.FromRank)],
                    this.Kind,
                    board[new File(this.File), new Rank(this.Rank)],
                    this.IsCheck,
                    this.IsCheckMate,
                    this.PromotedTo);
            }
        }
    }
}
