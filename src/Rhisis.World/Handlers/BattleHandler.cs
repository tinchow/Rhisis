using Ether.Network.Packets;
using Rhisis.Core.IO;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using Rhisis.World.Systems.Battle;

namespace Rhisis.World.Handlers
{
    internal static class BattleHandler
    {
        [PacketHandler(PacketType.MELEE_ATTACK)]
        public static void OnMeleeAttack(WorldClient client, INetPacketStream packet)
        {
            var attackMessage = packet.Read<int>();
            var objectId = packet.Read<int>();
            var unused = packet.Read<int>(); // Always 0
            var error = packet.Read<int>();
            var attackSpeed = packet.Read<float>();

            Logger.Debug("Player {0} attacks | {1}, {2}, {3}, {4}, {5}", 
                client.Player.Object.Name, attackMessage, objectId, unused, error, attackSpeed);

            // TODO: create event args for melee attack
            client.Player.NotifySystem<BattleSystem>(null);
        }
    }
}
