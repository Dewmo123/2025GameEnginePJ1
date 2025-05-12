using Core.EventSystem;
using DummyClient;
using Scripts.Network.Packets;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using UnityEngine;

namespace Scripts.Network
{
    public class NetworkManager : MonoBehaviour
    {
        [SerializeField] private EventChannelSO packetChannel;
        private static NetworkManager _instance;
        public static NetworkManager Instance => _instance;

        private Connector _connector;
        private ServerSession _session;
        private PacketQueue _packetQueue;
        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            else
                Destroy(gameObject);
            _connector = new Connector();
            _packetQueue = new PacketQueue(packetChannel);
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("172.31.0.186"), 7777);
            _connector.Connect(endPoint, () => _session = new ServerSession(_packetQueue), 1);
        }
        public void SendPacket(IPacket packet)
            => _session.Send(packet.Serialize());

        private void Update()
        {
            _packetQueue.FlushPackets(_session);
        }
    }
}