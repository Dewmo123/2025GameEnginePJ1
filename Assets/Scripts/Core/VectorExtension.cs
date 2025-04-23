using UnityEngine;

namespace Scripts.Core
{
    public static class VectorExtension
    {
        public static VectorPacket ToPacket(this Vector3 vector)
            => new VectorPacket() { x = vector.x, y = vector.y, z = vector.z };
        public static Vector3 ToVector3(this VectorPacket packet)
            => new Vector3() { x = packet.x, y = packet.y, z = packet.z };
    }
}
