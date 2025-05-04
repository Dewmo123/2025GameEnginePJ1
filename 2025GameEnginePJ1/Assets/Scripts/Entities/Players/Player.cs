using Scripts.Core;
using UnityEngine;

namespace Scripts.Entities.Players
{
    public class Player : NetworkEntity
    {
        public int Index { get; private set; } = 0;
        public bool IsOwner { get; private set; } = false;
        public bool isTest;

        public virtual void Init(LocationInfoPacket packet,bool isOwner)
        {
            Index = packet.index;
            IsOwner = isOwner;
            transform.position = packet.position.ToVector3();
            base.InitEntity();
        }
    }
}
