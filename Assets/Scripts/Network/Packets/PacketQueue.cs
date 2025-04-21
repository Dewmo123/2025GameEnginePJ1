using ServerCore;
using static Unity.Collections.AllocatorManager;
using System;
using System.Collections.Generic;
using DummyClient;

namespace Scripts.Network.Packets
{
    public class PacketQueue : Singleton<PacketQueue>
    {
        private object _lock = new object();
        private Queue<ArraySegment<byte>> _packets = new Queue<ArraySegment<byte>>();
        public void Push(ArraySegment<byte> packet)
        {
            lock (_lock)
            {
                _packets.Enqueue(packet);
            }
        }
        public void FlushPackets(ServerSession session)
        {
            lock (_lock)
            {
                Queue<ArraySegment<byte>> q;
                while (_packets.Count > 0)
                {
                    var packet = _packets.Dequeue();
                    PacketManager.Instance.OnRecvPacket(session, packet);
                }
            }
        }
    }
}
