using Scripts.Core;
using UnityEngine;

namespace Scripts.Entities.Players
{
    public class Player : NetworkEntity
    {
        public int Index { get; private set; } = 0;
        public bool IsOwner { get; private set; } = false;
        public virtual void Init(PlayerInfoPacket packet,bool isOwner)
        {
            Index = packet.index;
            IsOwner = isOwner;
            transform.position = packet.position.ToVector3();
            base.InitEntity();
        }
    }
}
