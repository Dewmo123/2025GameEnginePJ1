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
        private Vector3 _asd;
        private bool _asdas;
        private void Update()
        {
            if (_movement.IsAiming && !_asdas)
            {
                _asdas = true;
                Vector3 offset = _playerInput.GetWorldPosition() - owner.transform.position;
                _asd = _playerInput.GetWorldPosition();
                offset.y = 0f; // y축 무시 (수평 평면만 고려)
                float distance = offset.magnitude;
                float maxDistance = 3.0f; // 예시로 반지름 3
                if (distance > maxDistance)
                {
                    offset = offset.normalized * maxDistance;
                    transform.position = owner.transform.position + offset;
                }

            }
            else
            {
                transform.position = owner.transform.position;
            }
        }
    }
}
