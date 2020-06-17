// <copyright file="IRepository.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.DomainServices
{
    using System.Threading.Tasks;

    /// <summary>
    /// Generic repository contract.
    /// </summary>
    /// <typeparam name="TEntity">The entity type being persisted.</typeparam>
    /// <typeparam name="TId">The entity Id type.</typeparam>
    public interface IRepository<TEntity, TId>
    {
        /// <summary>
        /// Persists an entity.
        /// </summary>
        /// <param name="entity">The entity to persist.</param>
        /// <returns>The persisted entity.</returns>
        Task<TEntity> Persist(TEntity entity);

        /// <summary>
        /// Retrieves an entity from persistence.
        /// </summary>
        /// <param name="id">The Id of the entity.</param>
        /// <returns>The entity.</returns>
        Task<TEntity> Retrieve(TId id);

        /// <summary>
        /// Retrieves all entities from persistence.
        /// </summary>
        /// <param name="continuationToken">The continuation token to fetch the next page off of.</param>
        /// <returns>Retrieves a page of entity results.</returns>
        Task<ResultPage<TEntity>> RetrieveAll(string continuationToken = null);
    }
}
