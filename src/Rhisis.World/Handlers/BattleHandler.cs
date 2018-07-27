using Ether.Network.Packets;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using Rhisis.World.Systems.Battle;
using Rhisis.World.Systems.Battle.EventArgs;

namespace Rhisis.World.Handlers
{
    internal static class BattleHandler
    {
        [PacketHandler(PacketType.MELEE_ATTACK)]
        public static void OnMeleeAttack(WorldClient client, INetPacketStream packet)
        {
            var attackMessage = packet.Read<int>();
            var targetObjectId = packet.Read<int>();
            var unused = packet.Read<int>(); // Always 0
            var error = packet.Read<int>();
            var attackSpeed = packet.Read<float>();
            var battleEvent = new BattleMeleeEventArgs(attackMessage, targetObjectId, error, attackSpeed);
            
            client.Player.NotifySystem<BattleSystem>(battleEvent);
        }
    }
}
