using Scripts.Core;
using Scripts.Network;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UIElements;

namespace Scripts.Entities.Players.MyPlayers
{
    public class MyPlayerMovement : PlayerMovement
    {
        public override void Initialize(NetworkEntity entity)
        {
            base.Initialize(entity);
            _playerInput = (_player as MyPlayer).PlayerInput;
            _playerInput.OnAimEvent += HandleAim;
        }

        protected override void CalculateMovement()
        {
            _velocity = _direction * 3 * Time.fixedDeltaTime;
        }

        protected override void CalculateRotation()
        {
            if (IsAiming)
            {
                Vector3 dir = (_playerInput.GetWorldPosition() - transform.position).normalized;
                model.rotation = Quaternion.LookRotation(dir);
                float forwardDot = Vector3.Dot(model.forward, _direction); // 앞/뒤
                float rightDot = Vector3.Dot(model.right, _direction);     // 좌/우
                _animator.SetParam(_xHash, rightDot);
                _animator.SetParam(_zHash, forwardDot);
            }
            else
                model.rotation = _direction == Vector3.zero ? model.rotation
                    : Quaternion.LookRotation(_velocity);
        }
        private void Update()
        {
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
            Debug.Log($"other: {_player.CurrentAnimHash}");

            NetworkManager.Instance.SendPacket(info);
        }

    }
}
