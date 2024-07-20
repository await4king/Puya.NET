using System;

namespace Puya.Data.Events
{
    /// <summary>
    /// A container for passing entities that have been deleted. This is not used for entities that are deleted logicaly via a bit column.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class EntityDeletedEvent<TEntity> where TEntity : Puya.Data.BaseEntity
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="entity">Entity</param>
        public EntityDeletedEvent(TEntity entity)
        {
            this.Entity = entity;
        }

        /// <summary>
        /// Entity
        /// </summary>
        public TEntity Entity { get; }
    }
    /// <summary>
    /// A container for passing entities that have been deleted. This is not used for entities that are deleted logicaly via a bit column.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class EntityDeletedEvent<TEntity, PKType>
        where TEntity : Puya.Data.BaseEntity<PKType>
        where PKType : IComparable
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="entity">Entity</param>
        public EntityDeletedEvent(TEntity entity)
        {
            this.Entity = entity;
        }

        /// <summary>
        /// Entity
        /// </summary>
        public TEntity Entity { get; }
    }
}
