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
        private static NetworkManager _instance;
        public static NetworkManager Instance { get; private set; }

        private Connector _connector;
        private ServerSession _session;
        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            else
                Destroy(gameObject);
            _connector = new Connector();
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7777);
            _connector.Connect(endPoint, () => _session = new ServerSession(), 1);
        }

        private void Update()
        {
            PacketQueue.Instance.FlushPackets(_session);
        }
    }
}