using System;

namespace Puya.Data.Events
{
    /// <summary>
    /// A container for entities that are updated.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class EntityUpdatedEvent<TEntity> where TEntity : Puya.Data.BaseEntity
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="entity">Entity</param>
        public EntityUpdatedEvent(TEntity entity)
        {
            Entity = entity;
        }

        /// <summary>
        /// Entity
        /// </summary>
        public TEntity Entity { get; }
    }
    /// <summary>
    /// A container for entities that are updated.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class EntityUpdatedEvent<TEntity, PKType>
        where TEntity : Puya.Data.BaseEntity<PKType>
        where PKType : IComparable
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="entity">Entity</param>
        public EntityUpdatedEvent(TEntity entity)
        {
            Entity = entity;
        }

        /// <summary>
        /// Entity
        /// </summary>
        public TEntity Entity { get; }
    }
}
