using Scripts.Core;
using UnityEngine;

namespace Scripts.Entities.Players.OtherPlayers
{
    public class OtherPlayer : Player
    {
        public override void Init(LocationInfoPacket packet,bool  isOwner)
        {
            base.Init(packet,isOwner);
            var movement = GetCompo<OtherPlayerMovement>();
            transform.position = packet.position.ToVector3();
        }
        public void ShootBullet(Vector3 position, Vector3 direction)
        {

        }
    }
}
