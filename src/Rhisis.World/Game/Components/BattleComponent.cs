using Rhisis.World.Game.Core;

namespace Rhisis.World.Game.Components
{
    public sealed class BattleComponent
    {
        public bool IsFighting { get; set; }

        public IEntity Target { get; set; }
    }
}
