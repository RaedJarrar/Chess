// <copyright file="ResultPage.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.DomainServices
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines a page of results.
    /// </summary>
    /// <typeparam name="TResult">The result type.</typeparam>
    public class ResultPage<TResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResultPage{TResult}"/> class.
        /// </summary>
        /// <param name="results">The results.</param>
        /// <param name="continuationToken">An optional continuation token.</param>
        public ResultPage(IEnumerable<TResult> results, string continuationToken = default(string))
        {
            this.Results = results;
            this.ContinuationToken = continuationToken;
        }

        /// <summary>
        /// Gets the results in the current page.
        /// </summary>
        public IEnumerable<TResult> Results { get; }

        /// <summary>
        /// Gets a token used to fetch the next page of results.
        /// </summary>
        public string ContinuationToken { get; }

        /// <summary>
        /// Gets a value indicating whether this page is the last or not.
        /// </summary>
        public bool IsLastPage => string.IsNullOrEmpty(this.ContinuationToken);
    }
}
