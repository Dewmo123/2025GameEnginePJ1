using Scripts.Core;
using Scripts.Entities.Players.OtherPlayers;
using UnityEngine;

namespace Scripts.Entities.Players
{
    public class OtherPlayer : Player
    {
        public override void Init(PlayerInfoPacket packet,bool  isOwner)
        {
            base.Init(packet,isOwner);
            Vector2 vec = new Vector2(packet.direction.x, packet.direction.z);
            var movement = GetCompo<OtherPlayerMovement>();
            movement.SetMovement(vec);
            movement.SetServerPosition(packet.position.ToVector3());
        }
    }
}
