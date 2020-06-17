// <copyright file="Player.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.Domain
{
    using System;

    /// <summary>
    /// Represents a player.
    /// </summary>
    public class Player : AggregateRoot
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        public Player()
            : this(new Random().Next())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="id">The player Id.</param>
        public Player(long id)
        {
            this.Id = id;
        }

        /// <summary>
        /// Gets or sets the name of the player.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the rating of the player.
        /// </summary>
        public int Rating { get; set; }
    }
}
