using System;
using System.Linq.Expressions;
using Rhisis.Core.IO;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Entities;
using Rhisis.World.Systems.Battle.EventArgs;

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
            switch (e)
            {
                case BattleMeleeEventArgs meleeEventArgs:
                    this.ProcessMeleeAttack(entity, meleeEventArgs);
                    break;
            }
        }

        private void ProcessMeleeAttack(IEntity attacker, BattleMeleeEventArgs e)
        {
            var defender = attacker.FindEntity<IEntity>(e.TargetId);

            if (defender == null)
                throw new SystemException($"Cannot find taget id: {e.TargetId}");

            Logger.Debug("Player {0} is attacking {1}", attacker.Object.Name, defender.Object.Name);
        }

        private void Attack(IEntity attacker, IEntity defender)
        {
            int damages = this.GetRealDamages(attacker, defender);

            Logger.Debug("{0} inflicts {1} damages to {2}", attacker.Object.Name, damages, defender.Object.Name);
        }

        private int GetRealDamages(IEntity attacker, IEntity defender) => this.ReduceDamages(defender, this.GetDamages(attacker));

        private int GetDamages(IEntity attacker)
        {
            // TODO: Get the damages of the attacker (including bonuses and buffs)

            return 0;
        }

        private int ReduceDamages(IEntity defender, int damages)
        {
            // TODO: Reduce the damages based on the defense of the entity.

            return 0;
        }
    }
}
