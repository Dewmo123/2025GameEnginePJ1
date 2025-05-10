using ServerCore;
using static Unity.Collections.AllocatorManager;
using System;
using System.Collections.Generic;
using DummyClient;
using Core.EventSystem;
using UnityEngine;

namespace Scripts.Network.Packets
{
    public class PacketQueue
    {
        private object _lock = new object();
        private Queue<IPacket> _packets = new Queue<IPacket>();
        private PacketManager _packetManager;
        public PacketQueue(EventChannelSO eventChannel)
        {
            _packetManager = new PacketManager(eventChannel);
        }

        public void Push(ArraySegment<byte> packet)
        {
            lock (_lock)
            {
                var pkt = _packetManager.OnRecvPacket(packet);
                _packets.Enqueue(pkt);
            }
        }
        public void FlushPackets(ServerSession session)
        {
            while (true)
            {
                lock (_lock)
                {
                    if (_packets.Count <= 0)
                        break;

                    IPacket packet = _packets.Dequeue();
                    _packetManager.HandlePacket(session, packet);
                }
            }
        }
    }
}
