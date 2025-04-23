using Scripts.Core;
using Scripts.Network;
using UnityEngine;

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
            _velocity = _direction * 3 * Time.deltaTime;

            if (IsAiming)
            {
                Vector3 dir = (_playerInput.GetWorldPosition() - transform.position).normalized;
                model.rotation = Quaternion.Euler(dir);
            }
            else
                model.rotation = _direction == Vector3.zero ? model.rotation
                    : Quaternion.Euler(_direction.x, 0, _direction.z);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            SendMyInfo();
        }
        private void SendMyInfo()
        {
            C_UpdateInfo info = new C_UpdateInfo()
            {
                playerInfo = new PlayerInfoPacket()
                {
                    direction = _direction.ToPacket(),
                    position = _player.transform.position.ToPacket(),
                    index = _player.Index,
                    isAiming = IsAiming,
                    mouse = _playerInput.GetWorldPosition().ToPacket()
                }
            };
            NetworkManager.Instance.SendPacket(info);
        }

    }
}
