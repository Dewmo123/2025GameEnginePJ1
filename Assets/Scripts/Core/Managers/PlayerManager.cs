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
        public int MyIndex { get; private set; }

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
        public void InitPlayer(S_EnterRoomFirst packet)
        {
            MyIndex = packet.myIndex;
            foreach (var info in packet.playerInfos)
            {
                Player player;
                if (info.index == MyIndex)
                {
                    player = Instantiate(myPlayer).GetComponent<Player>();
                    cinemachine.Target.TrackingTarget = player.transform;
                }
                else
                    player = Instantiate(otherPlayer).GetComponent<Player>();
                player.Init(info, info.index == MyIndex);
                player.GetComponentInChildren<PlayerMovement>().SetPosition(info.position.ToVector3());
                _players.Add(info.index, player);
            }
        }
        public Player GetPlayerById(int index)
            => _players.GetValueOrDefault(index);
    }
}