// <copyright file="File.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.Domain
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines a file in a chess board. A file is a column. There are 8 files in a chess board starting from A and ending in H.
    /// </summary>
    public sealed class File : ValueObject<File>
    {
        /// <summary>
        /// The A file.
        /// </summary>
        public static readonly File A = new File(1);

        /// <summary>
        /// The B file.
        /// </summary>
        public static readonly File B = new File(2);

        /// <summary>
        /// The C file.
        /// </summary>
        public static readonly File C = new File(3);

        /// <summary>
        /// The D file.
        /// </summary>
        public static readonly File D = new File(4);

        /// <summary>
        /// The E file.
        /// </summary>
        public static readonly File E = new File(5);

        /// <summary>
        /// The F file.
        /// </summary>
        public static readonly File F = new File(6);

        /// <summary>
        /// The G file.
        /// </summary>
        public static readonly File G = new File(7);

        /// <summary>
        /// The H file.
        /// </summary>
        public static readonly File H = new File(8);

        /// <summary>
        /// Gets a collection of all files.
        /// </summary>
        public static readonly IReadOnlyCollection<File> Files = new File[]
        {
            File.A,
            File.B,
            File.C,
            File.D,
            File.E,
            File.F,
            File.G,
            File.H,
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="File"/> class.
        /// </summary>
        /// <param name="index">The index of the file.</param>
        public File(int index)
        {
            switch (index)
            {
                case 1:
                    this.Name = "A";
                    break;
                case 2:
                    this.Name = "B";
                    break;
                case 3:
                    this.Name = "C";
                    break;
                case 4:
                    this.Name = "D";
                    break;
                case 5:
                    this.Name = "E";
                    break;
                case 6:
                    this.Name = "F";
                    break;
                case 7:
                    this.Name = "G";
                    break;
                case 8:
                    this.Name = "H";
                    break;
                default:
                    throw new ArgumentException($"{index} is out of range. Allowed values are between 1 and 8.", nameof(index));
            }

            this.Index = index;
        }

        /// <summary>
        /// Gets the index of the file.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Increments a file.
        /// </summary>
        /// <param name="file">The file to increment.</param>
        /// <returns>The incremented file or null if we ran out of files.</returns>
        public static File operator ++(File file)
        {
            if (file.Index >= 8)
            {
                return null;
            }

            return new File(file.Index + 1);
        }

        /// <summary>
        /// Decrements a file.
        /// </summary>
        /// <param name="file">The file to decrement.</param>
        /// <returns>The decremented file or null if we ran out of files.</returns>
        public static File operator --(File file)
        {
            if (file.Index <= 1)
            {
                return null;
            }

            return new File(file.Index - 1);
        }

        /// <summary>
        /// Adds a number to a file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="increment">The increment.</param>
        /// <returns>The resulting file or null if out of bounds.</returns>
        public static File operator +(File file, int increment)
        {
            int newFileIndex = file.Index + increment;

            if (newFileIndex < 1 || newFileIndex > 8)
            {
                return null;
            }

            return new File(newFileIndex);
        }

        /// <summary>
        /// Subtracts a number from a file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="increment">The decrement.</param>
        /// <returns>The resulting file or null if out of bounds.</returns>
        public static File operator -(File file, int increment)
        {
            int newFileIndex = file.Index - increment;

            if (newFileIndex < 1 || newFileIndex > 8)
            {
                return null;
            }

            return new File(newFileIndex);
        }

        /// <summary>
        /// Checks if a file is greater than another.
        /// </summary>
        /// <param name="a">The first file.</param>
        /// <param name="b">The second file.</param>
        /// <returns>True if the first file is greater than second.</returns>
        public static bool operator >(File a, File b)
        {
            return a.Index > b.Index;
        }

        /// <summary>
        /// Checks if a file is greater than or equal to another.
        /// </summary>
        /// <param name="a">The first file.</param>
        /// <param name="b">The second file.</param>
        /// <returns>True if the first file is greater than or equal to second.</returns>
        public static bool operator >=(File a, File b)
        {
            return a.Index >= b.Index;
        }

        /// <summary>
        /// Checks if a file is less than another.
        /// </summary>
        /// <param name="a">The first file.</param>
        /// <param name="b">The second file.</param>
        /// <returns>True if the first file is less than second.</returns>
        public static bool operator <(File a, File b)
        {
            return a.Index < b.Index;
        }

        /// <summary>
        /// Checks if a file is less than or equal to another.
        /// </summary>
        /// <param name="a">The first file.</param>
        /// <param name="b">The second file.</param>
        /// <returns>True if the first file is less than or equal to second.</returns>
        public static bool operator <=(File a, File b)
        {
            return a.Index <= b.Index;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.Name;
        }

        /// <inheritdoc/>
        protected override bool EqualsCore(File other)
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
