// <copyright file="GameRepository.ArmyDto.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.DomainServices
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using Chess.Domain;

    /// <summary>
    /// Manages game persistence.
    /// </summary>
    public partial class GameRepository
    {
        [DataContract]
        private class ArmyDto
        {
            public ArmyDto()
            {
            }

            public ArmyDto(Army army)
            {
                this.Id = army.Id;
                this.CommanderId = army.Commander.Id;
                this.Kind = army.Kind;
                this.Pieces = army.Pieces?.Select(p => new PieceDto
                {
                    Id = p.Id,
                    Kind = p.Kind,
                    File = p.Position.File.Index,
                    Rank = p.Position.Rank.Index,
                    Status = p.Status,
                });
            }

            [DataMember]
            public long Id { get; set; }

            [DataMember]
            public long CommanderId { get; set; }

            [DataMember]
            public ArmyKind Kind { get; set; }

            [DataMember]
            public IEnumerable<PieceDto> Pieces { get; set; }
        }
    }
}
