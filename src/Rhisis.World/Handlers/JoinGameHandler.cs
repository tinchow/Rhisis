﻿using Ether.Network.Packets;
using Rhisis.Core.Common;
using Rhisis.Core.IO;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using Rhisis.Core.Network.Packets.World;
using Rhisis.Core.Structures;
using Rhisis.Database;
using Rhisis.Database.Structures;
using Rhisis.World.Core.Components;
using Rhisis.World.Packets;

namespace Rhisis.World.Handlers
{
    public static class JoinGameHandler
    {
        [PacketHandler(PacketType.JOIN)]
        public static void OnJoin(WorldClient client, NetPacketBase packet)
        {
            var joinPacket = new JoinPacket(packet);
            Character character = null;

            using (var db = DatabaseService.GetContext())
            {
                character = db.Characters.Get(joinPacket.PlayerId);
            }

            if (character == null)
            {
                // This is an hack attempt
                return;
            }

            if (character.User.Authority <= 0)
            {
                // Account banned so he can't connect to the game.
                return;
            }

            var map = WorldServer.Maps[character.MapId];

            // 1st: Create the player entity with the map context
            client.Player = map.Context.CreateEntity();

            // 2nd: create the components
            var objectComponent = new ObjectComponent
            {
                ModelId = character.Gender == 0 ? 11 : 12,
                Type = WorldObjectType.Mover,
                EntityType = WorldEntityType.Player,
                MapId = character.MapId,
                Position = new Vector3(character.PosX, character.PosY, character.PosZ),
                Angle = character.Angle,
                Size = 100,
                Name = character.Name,
                Spawned = false
            };

            var humanComponent = new HumanComponent
            {
                Gender = character.Gender,
                SkinSetId = character.SkinSetId,
                HairId = character.HairId,
                HairColor = character.HairColor,
                FaceId = character.FaceId,
            };

            var playerComponent = new PlayerComponent
            {
                Id = character.Id,
                Slot = character.Slot,
                Connection = client
            };

            var movableComponent = new MovableComponent
            {
                Speed = WorldServer.Movers[objectComponent.ModelId].Speed,
                DestinationPosition = objectComponent.Position.Clone(),
                LastMoveTime = Time.GetElapsedTime(),
                NextMoveTime = Time.GetElapsedTime() + 10
            };

            // 3rd: attach the component to the entity
            client.Player.AddComponent(objectComponent);
            client.Player.AddComponent(humanComponent);
            client.Player.AddComponent(playerComponent);
            client.Player.AddComponent(movableComponent);

            // 4rd: spawn the player
            WorldPacketFactory.SendPlayerSpawn(client, client.Player);

            // 5th: player is now spawned
            objectComponent.Spawned = true;
        }
    }
}
