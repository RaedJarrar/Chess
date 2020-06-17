// <copyright file="GameRepository.GameDto.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.DomainServices
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Chess.Domain;

    /// <summary>
    /// Manages game persistence.
    /// </summary>
    public partial class GameRepository
    {
        [DataContract]
        private class GameDto
        {
            [DataMember]
            public long Id { get; set; }

            [DataMember]
            public DateTime CreatedAt { get; set; }

            [DataMember]
            public GameWinner Winner { get; set; }

            [DataMember]
            public ArmyKind Turn { get; set; }

            [DataMember]
            public GameStatus Status { get; set; }

            [DataMember]
            public IEnumerable<MovePairDto> Moves { get; set; }

            [DataMember]
            public ArmyDto WhiteArmy { get; set; }

            [DataMember]
            public ArmyDto BlackArmy { get; set; }
        }
    }
}
