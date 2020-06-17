// <copyright file="PlayerDto.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.DomainServices
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Player repository.
    /// </summary>
    public partial class PlayerRepository
    {
        /// <summary>
        /// A player data transfer object used to represent a player in persistence.
        /// </summary>
        [DataContract]
        private class PlayerDto
        {
            /// <summary>
            /// Gets or sets the player Id.
            /// </summary>
            [DataMember]
            public long Id { get; set; }

            /// <summary>
            /// Gets or sets the player name.
            /// </summary>
            [DataMember]
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets the player rate.
            /// </summary>
            [DataMember]
            public int Rating { get; set; }
        }
    }
}
