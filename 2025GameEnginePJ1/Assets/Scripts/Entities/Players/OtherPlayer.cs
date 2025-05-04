using Scripts.Core;
using Scripts.Entities.Players.OtherPlayers;
using UnityEngine;

namespace Scripts.Entities.Players
{
    public class OtherPlayer : Player
    {
        public override void Init(LocationInfoPacket packet,bool  isOwner)
        {
            base.Init(packet,isOwner);
            var movement = GetCompo<OtherPlayerMovement>();
            transform.position = packet.position.ToVector3();
        }
    }
}
