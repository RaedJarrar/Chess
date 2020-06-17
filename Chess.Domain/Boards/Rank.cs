// <copyright file="Rank.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.Domain
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines a rank in a chess board. A rank is a row. There are 8 ranks in a chess board.
    /// </summary>
    public sealed class Rank : ValueObject<Rank>
    {
        /// <summary>
        /// The first rank.
        /// </summary>
        public static readonly Rank First = new Rank(1);

        /// <summary>
        /// The second rank.
        /// </summary>
        public static readonly Rank Second = new Rank(2);

        /// <summary>
        /// The third rank.
        /// </summary>
        public static readonly Rank Third = new Rank(3);

        /// <summary>
        /// The fourth rank.
        /// </summary>
        public static readonly Rank Fourth = new Rank(4);

        /// <summary>
        /// The fifth rank.
        /// </summary>
        public static readonly Rank Fifth = new Rank(5);

        /// <summary>
        /// The sixth rank.
        /// </summary>
        public static readonly Rank Sixth = new Rank(6);

        /// <summary>
        /// The seventh rank.
        /// </summary>
        public static readonly Rank Seventh = new Rank(7);

        /// <summary>
        /// The eighth rank.
        /// </summary>
        public static readonly Rank Eighth = new Rank(8);

        /// <summary>
        /// Gets a collection of all ranks.
        /// </summary>
        public static readonly IReadOnlyCollection<Rank> Ranks = new Rank[]
        {
            Rank.First,
            Rank.Second,
            Rank.Third,
            Rank.Fourth,
            Rank.Fifth,
            Rank.Sixth,
            Rank.Seventh,
            Rank.Eighth,
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="Rank"/> class.
        /// </summary>
        /// <param name="index">The index of the rank.</param>
        public Rank(int index)
        {
            switch (index)
            {
                case 1:
                    this.Name = "1";
                    break;
                case 2:
                    this.Name = "2";
                    break;
                case 3:
                    this.Name = "3";
                    break;
                case 4:
                    this.Name = "4";
                    break;
                case 5:
                    this.Name = "5";
                    break;
                case 6:
                    this.Name = "6";
                    break;
                case 7:
                    this.Name = "7";
                    break;
                case 8:
                    this.Name = "8";
                    break;
                default:
                    throw new ArgumentException($"{index} is out of range. Allowed values are between 1 and 8.", nameof(index));
            }

            this.Index = index;
        }

        /// <summary>
        /// Gets the index of the rank.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Gets the name of the rank.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Increments a rank.
        /// </summary>
        /// <param name="rank">The rank to increment.</param>
        /// <returns>The incremented rank or null if we ran out of ranks.</returns>
        public static Rank operator ++(Rank rank)
        {
            if (rank.Index >= 8)
            {
                return null;
            }

            return new Rank(rank.Index + 1);
        }

        /// <summary>
        /// Decrements a rank.
        /// </summary>
        /// <param name="rank">The rank to decrement.</param>
        /// <returns>The decremented rank or null if we ran out of ranks.</returns>
        public static Rank operator --(Rank rank)
        {
            if (rank.Index <= 1)
            {
                return null;
            }

            return new Rank(rank.Index - 1);
        }

        /// <summary>
        /// Adds a number to a rank.
        /// </summary>
        /// <param name="rank">The rank.</param>
        /// <param name="increment">The increment.</param>
        /// <returns>The resulting rank or null if out of bounds.</returns>
        public static Rank operator +(Rank rank, int increment)
        {
            int newRankIndex = rank.Index + increment;

            if (newRankIndex < 1 || newRankIndex > 8)
            {
                return null;
            }

            return new Rank(newRankIndex);
        }

        /// <summary>
        /// Subtracts a number from a rank.
        /// </summary>
        /// <param name="rank">The rank.</param>
        /// <param name="increment">The decrement.</param>
        /// <returns>The resulting rank or null if out of bounds.</returns>
        public static Rank operator -(Rank rank, int increment)
        {
            int newRankIndex = rank.Index - increment;

            if (newRankIndex < 1 || newRankIndex > 8)
            {
                return null;
            }

            return new Rank(newRankIndex);
        }

        /// <summary>
        /// Checks if a rank is greater than another.
        /// </summary>
        /// <param name="a">The first rank.</param>
        /// <param name="b">The second rank.</param>
        /// <returns>True if the first rank is greater than second.</returns>
        public static bool operator >(Rank a, Rank b)
        {
            return a.Index > b.Index;
        }

        /// <summary>
        /// Checks if a rank is greater than or equal to another.
        /// </summary>
        /// <param name="a">The first rank.</param>
        /// <param name="b">The second rank.</param>
        /// <returns>True if the first rank is greater than or equal to second.</returns>
        public static bool operator >=(Rank a, Rank b)
        {
            return a.Index >= b.Index;
        }

        /// <summary>
        /// Checks if a rank is less than another.
        /// </summary>
        /// <param name="a">The first rank.</param>
        /// <param name="b">The second rank.</param>
        /// <returns>True if the first rank is less than second.</returns>
        public static bool operator <(Rank a, Rank b)
        {
            return a.Index < b.Index;
        }

        /// <summary>
        /// Checks if a rank is less than or equal to another.
        /// </summary>
        /// <param name="a">The first rank.</param>
        /// <param name="b">The second rank.</param>
        /// <returns>True if the first rank is less than or equal to second.</returns>
        public static bool operator <=(Rank a, Rank b)
        {
            return a.Index <= b.Index;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.Name;
        }

        /// <inheritdoc/>
        protected override bool EqualsCore(Rank other)
        {
            return this.Index == other.Index;
        }

        /// <inheritdoc/>
        protected override int GetHashCodeCore()
        {
            return this.Index.GetHashCode();
        }
    }
}
