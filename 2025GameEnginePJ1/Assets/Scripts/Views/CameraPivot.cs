using Scripts.Core.GameSystem;
using Scripts.Entities.Players;
using Scripts.Entities.Players.MyPlayers;
using System.Runtime.InteropServices;
using System;
using Unity.Cinemachine;
using UnityEngine;

namespace Scripts.Views
{
    public class CameraPivot : MonoBehaviour
    {
        [SerializeField] private MyPlayer owner;
        [SerializeField] private CinemachineCamera followCam;
        [SerializeField] private float maxDistance;
        private PlayerInputSO _playerInput;
        private MyPlayerMovement _movement;

        private void Start()
        {
            _movement = owner.GetCompo<MyPlayerMovement>();
            _playerInput = owner.PlayerInput;
        }
        private Vector3 _before;
        private bool _asdas;
        private void Update()
        {
            Vector3 dir = (_playerInput.GetWorldPosition() - _before).normalized;
            Quaternion rotation = Quaternion.LookRotation(dir);
            followCam.transform.rotation = rotation;
            
        }
    }
}
