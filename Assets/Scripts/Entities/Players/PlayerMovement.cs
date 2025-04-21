using Assets.Scripts.Entities;
using Scripts.Core.GameSystem;
using UnityEngine;

namespace Scripts.Entities.Players
{
    public class PlayerMovement : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private Transform model;
        private PlayerInputSO _playerInput;
        private Player _player;
        private EntityAnimator _animator;
        public void Initialize(Entity entity)
        {
            _player = entity as Player;
            _playerInput = _player.PlayerInput;
            _animator = entity.GetCompo<EntityAnimator>();
        }
        public void SetDirection(Vector3 dir)
        {
            model.rotation = Quaternion.Euler(dir.x, 0, dir.z);
        }
    }
}
