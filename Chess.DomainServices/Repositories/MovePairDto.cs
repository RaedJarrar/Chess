// <copyright file="MovePairDto.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.DomainServices
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Manages game persistence.
    /// </summary>
    public partial class GameRepository
    {
        [DataContract]
        private class MovePairDto
        {
            [DataMember]
            public MoveDto WhiteMove { get; set; }

            [DataMember]
            public MoveDto BlackMove { get; set; }
        }
    }
}
