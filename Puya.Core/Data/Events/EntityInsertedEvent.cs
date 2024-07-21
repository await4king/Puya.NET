using System;

namespace Puya.Data.Events
{
    /// <summary>
    /// A container for entities that have been inserted.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class EntityInsertedEvent<TEntity> where TEntity : Puya.Data.BaseEntity
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="entity">Entity</param>
        public EntityInsertedEvent(TEntity entity)
        {
            Entity = entity;
        }

        /// <summary>
        /// Entity
        /// </summary>
        public TEntity Entity { get; }
    }
    /// <summary>
    /// A container for entities that have been inserted.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class EntityInsertedEvent<TEntity, PKType>
        where TEntity : Puya.Data.BaseEntity<PKType>
        where PKType : IComparable
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="entity">Entity</param>
        public EntityInsertedEvent(TEntity entity)
        {
            Entity = entity;
        }

        /// <summary>
        /// Entity
        /// </summary>
        public TEntity Entity { get; }
    }
}
