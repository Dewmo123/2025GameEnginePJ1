using Scripts.Entities.Players;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

namespace Scripts.Core.Managers
{
    public class PlayerManager : MonoBehaviour
    {
        private static PlayerManager _instance;
        public static PlayerManager Instance => _instance;
        public int MyIndex { get; private set; } = 0;

        [SerializeField] private GameObject myPlayer;
        [SerializeField] private GameObject otherPlayer;
        [SerializeField] private CinemachineCamera cinemachine;
        private Dictionary<int, Player> _players;
        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            else
                Destroy(gameObject);
            _players = new Dictionary<int, Player>();
        }
        public void FirstInitPlayer(S_EnterRoomFirst packet)
        {
            MyIndex = packet.myIndex;
            foreach (var info in packet.playerInfos)
            {
                if (info.index == MyIndex)
                    InitMyPlayer(info);
                else
                    InitOtherPlayer(info);
            }
        }
        public void InitOtherPlayer(PlayerInfoPacket packet)
        {
            Debug.Log($"InitOtherPlayer: {packet.index}");
            if (MyIndex==0||MyIndex == packet.index)
                return;
            Player player = Instantiate(otherPlayer).GetComponent<Player>();
            player.Init(packet, false);
            _players.Add(packet.index, player);
        }
        public void InitMyPlayer(PlayerInfoPacket packet)
        {
            Debug.Log($"InitMyPlayer: {packet.index}");
            var player = Instantiate(myPlayer).GetComponent<Player>();
            cinemachine.Target.TrackingTarget = player.transform;
            player.Init(packet, true);
            _players.Add(packet.index, player);
            player.GetComponentInChildren<PlayerMovement>().SetPosition(packet.position.ToVector3());
        }
        public Player GetPlayerById(int index)
            => _players.GetValueOrDefault(index);
    }
}