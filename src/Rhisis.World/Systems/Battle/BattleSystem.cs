using Rhisis.World.Game.Core;

namespace Rhisis.World.Systems.Battle
{
    [System]
    public sealed class BattleSystem : NotifiableSystemBase
    {
        public BattleSystem(IContext context) 
            : base(context)
        {
        }

        public override void Execute(IEntity entity, SystemEventArgs e)
        {
            // TODO
        }
    }
}
