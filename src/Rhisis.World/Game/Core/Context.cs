﻿using Rhisis.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Game.Core
{
    public abstract class Context : IContext
    {
        protected static readonly object SyncRoot = new object();

        private readonly IDictionary<int, IEntity> _entities;
        private readonly IList<ISystem> _systems;
        private bool _disposedValue;

        /// <inheritdoc />
        public double GameTime { get; protected set; }

        /// <inheritdoc />
        public IEnumerable<IEntity> Entities => this._entities.Values;

        /// <inheritdoc />
        public IEnumerable<ISystem> Systems => this._systems;

        /// <summary>
        /// Creates a new <see cref="Context"/> instance.
        /// </summary>
        public Context()
        {
            this._entities = new Dictionary<int, IEntity>();
            this._systems = new List<ISystem>();
        }

        /// <inheritdoc />
        public TEntity CreateEntity<TEntity>() where TEntity : IEntity
        {
            var entity = Activator.CreateInstance(typeof(TEntity), this) as IEntity;

            if (entity == null)
                throw new RhisisException($"An error occured while creating an entity of type {typeof(TEntity)}");

            lock (SyncRoot)
            {
                if (!this._entities.TryAdd(entity.Id, entity))
                    throw new RhisisException($"An error occured while adding the entity to the context list.");
            }

            return (TEntity)entity;
        }

        /// <inheritdoc />
        public bool DeleteEntity(int id)
        {
            bool removed = false;

            lock (SyncRoot)
            {
                removed = this._entities.Remove(id);
            }
            
            return removed;
        }

        /// <inheritdoc />
        public bool DeleteEntity(IEntity entity) => this.DeleteEntity(entity.Id);

        /// <inheritdoc />
        public TEntity FindEntity<TEntity>(int id) where TEntity : IEntity
        {
            return this._entities.TryGetValue(id, out IEntity entity) ? (TEntity)entity : default(TEntity);
        }

        /// <inheritdoc />
        public abstract void Update();

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose the context resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposedValue)
            {
                if (disposing)
                {
                    foreach (var entity in this._entities)
                        entity.Value.Dispose();

                    this._entities.Clear();
                    this._systems.Clear();
                }

                this._disposedValue = true;
            }
        }

        /// <inheritdoc />
        public void NotifySystem<TSystem>(IEntity entity, SystemEventArgs e)
        {
            ISystem system = this.Systems.FirstOrDefault(x => x.GetType() == typeof(TSystem));

            if (system == null)
                throw new RhisisException($"Cannot find system with type: {typeof(TSystem).FullName} within current context.");

            if (!(system is INotifiableSystem notifiableSystem))
                throw new RhisisException($"This system {system.GetType().Name} is not a notifiable system.");

            notifiableSystem.Execute(entity, e);
        }

        /// <inheritdoc />
        public void AddSystem(ISystem system) => this._systems.Add(system);

        /// <inheritdoc />
        public void RemoveSystem(ISystem system) => this._systems.Remove(system);
    }
}