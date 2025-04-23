using Scripts.Core;
using UnityEngine;

namespace Scripts.Entities.Players.OtherPlayers
{
    public class OtherPlayerMovement : PlayerMovement
    {
        [SerializeField] private float syncOffset;
        private Vector3 _mousePos;
        public void Synchronize(PlayerInfoPacket packet)
        {
            SetMovement(packet.direction.ToVector3());
            SetServerPosition(packet.position.ToVector3());
            HandleAim(packet.isAiming);
            _mousePos = packet.mouse.ToVector3();
        }
        protected override void CalculateMovement()
        {
            _velocity = _direction * 3 * Time.deltaTime;

            if (Vector3.Distance(_player.transform.position, _serverPosition) > syncOffset)
                SetPosition(_serverPosition);
        }
    }
}