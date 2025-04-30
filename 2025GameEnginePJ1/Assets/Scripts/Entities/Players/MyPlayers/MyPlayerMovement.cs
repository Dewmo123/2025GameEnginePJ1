using Scripts.Core;
using Scripts.Network;
using System;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UIElements;

namespace Scripts.Entities.Players.MyPlayers
{
    public class MyPlayerMovement : PlayerMovement
    {
        public enum WalkMode
        {
            Idle,
            Walk,
            Sprint,
            Aim
        }
        public bool IsSprint { get; protected set; }
        [SerializeField] private float walkSpeed;
        [SerializeField] private float sprintSpeed;
        [SerializeField] private float aimSpeed;
        private float _currentSpeed;
        public override void Initialize(NetworkEntity entity)
        {
            base.Initialize(entity);
            _playerInput = _player.PlayerInput;
            _playerInput.OnAimEvent += HandleAim;
            _playerInput.OnSprintEvent += HandleSprint;
            _currentSpeed = walkSpeed;
        }

        public void SetMoveState(WalkMode state)
        {
            _currentSpeed = state switch
            {
                WalkMode.Idle => 0,
                WalkMode.Walk => walkSpeed,
                WalkMode.Aim => aimSpeed,
                WalkMode.Sprint => sprintSpeed,
                _ => 0
            };
        }
        private void OnDestroy()
        {
            _playerInput.OnSprintEvent -= HandleSprint;
            _playerInput.OnAimEvent -= HandleAim;
        }
        private void HandleSprint(bool obj)
            => IsSprint = obj;
        public void HandleAim(bool obj)
            => IsAiming = obj;
        protected override void CalculateMovement()
        {
            _velocity = _direction * _currentSpeed * Time.fixedDeltaTime;
        }

        protected override void CalculateRotation()
        {
            Quaternion rotation;
            if (IsAiming)
            {
                Vector3 dir = (_playerInput.GetWorldPosition() - transform.position).normalized;
                dir.y = 0;
                rotation = Quaternion.LookRotation(dir);
                float forwardDot = Vector3.Dot(model.forward, _direction); // 앞/뒤
                float rightDot = Vector3.Dot(model.right, _direction);     // 좌/우
                _animator.SetParam(_xHash, rightDot);
                _animator.SetParam(_zHash, forwardDot);
            }
            else
            {
                rotation = _direction == Vector3.zero ? model.rotation
                   : Quaternion.LookRotation(_velocity);
            }
            float rotateSpeed = 20f;
            model.rotation = Quaternion.Lerp(model.rotation, rotation, Time.fixedDeltaTime * rotateSpeed);
        }
        private void Update()
        {
            if (!_player.isTest)
                SendMyInfo();
        }
        private void SendMyInfo()
        {
            C_UpdateInfo info = new C_UpdateInfo()
            {
                playerInfo = new PlayerInfoPacket()
                {
                    rotation = model.rotation.ToPacket(),
                    position = _player.transform.position.ToPacket(),
                    index = _player.Index,
                    isAiming = IsAiming,
                    mouse = _playerInput.GetWorldPosition().ToPacket(),
                    animHash = _player.CurrentAnimHash
                }
            };

            NetworkManager.Instance.SendPacket(info);
        }

    }
}
