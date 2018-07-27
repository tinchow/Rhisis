using Rhisis.Core.Structures;
using Rhisis.World.Game.Core;

namespace Rhisis.World.Game.Components
{
    public class MovableComponent
    {
        public long LastMoveTime { get; set; }

        public long NextMoveTime { get; set; }

        public Vector3 DestinationPosition { get; set; }

        public float Speed { get; set; }

        public IEntity FollowedTarget { get; set; }

        public bool IsFollowing => this.FollowedTarget != null;

        public bool HasArrived { get; set; }

        public MovableComponent()
        {
            this.DestinationPosition = new Vector3();
        }

        public override string ToString()
        {
            return $"Dest Position: {this.DestinationPosition.ToString()}";
        }
    }
}
