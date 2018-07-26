using Rhisis.World.Game.Core;

namespace Rhisis.World.Systems.Battle.EventArgs
{
    public sealed class BattleMeleeEventArgs : SystemEventArgs
    {
        public int AttackMessage { get; }

        public int TargetId { get; }

        public int ErrorMessage { get; }

        public float AttackSpeed { get; }

        public BattleMeleeEventArgs(int attackMessage, int targetId, int errorMessage, float attackSpeed)
        {
            this.AttackMessage = attackMessage;
            this.TargetId = targetId;
            this.ErrorMessage = errorMessage;
            this.AttackSpeed = attackSpeed;
        }

        public override bool CheckArguments()
        {
            return this.TargetId > 0;
        }
    }
}
