// <copyright file="GameRepository.PieceDto.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.DomainServices
{
    using System.Runtime.Serialization;
    using Chess.Domain;

    /// <summary>
    /// Manages game persistence.
    /// </summary>
    public partial class GameRepository
    {
        [DataContract]
        private class PieceDto
        {
            [DataMember]
            public long Id { get; set; }

            [DataMember]
            public PieceKind Kind { get; set; }

            [DataMember]
            public PieceStatus Status { get; set; }

            [DataMember]
            public int File { get; set; }

            [DataMember]
            public int Rank { get; set; }

            public IPieceSpecification ToPieceSpecification(ChessBoard board)
            {
                return new PieceSpecification
                {
                    Id = this.Id,
                    Kind = this.Kind,
                    Status = this.Status,
                    Position = board[new Domain.File(this.File), new Rank(this.Rank)],
                };
            }
        }
    }
}
