using System;
using System.Linq.Expressions;
using Rhisis.World.Game.Core;

namespace Rhisis.World.Systems.Battle
{
    [System]
    public sealed class BattleSystem : NotifiableSystemBase
    {
        /// <inheritdoc />
        protected override Expression<Func<IEntity, bool>> Filter => 
            x => x.Type == WorldEntityType.Player || x.Type == WorldEntityType.Monster;

        /// <summary>
        /// Creates a new <see cref="BattleSystem"/> instance.
        /// </summary>
        /// <param name="context">Context</param>
        public BattleSystem(IContext context) 
            : base(context)
        {
        }

        /// <inheritdoc />
        public override void Execute(IEntity entity, SystemEventArgs e)
        {
            // TODO
        }
    }
}
