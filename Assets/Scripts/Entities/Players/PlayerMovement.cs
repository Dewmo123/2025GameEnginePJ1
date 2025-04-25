using Assets.Scripts.Entities;
using Scripts.Core;
using Scripts.Core.GameSystem;
using Scripts.Entities.Players.MyPlayers;
using Scripts.Network;
using System;
using UnityEngine;

namespace Scripts.Entities.Players
{
    //Movement를 My,Other 두개로 나누려고 추상화 했는데 결국 other에선 위치동기화만 해버린
    public abstract class PlayerMovement : MonoBehaviour, IEntityComponent
    {
        [SerializeField] protected Transform model;
        [SerializeField] private float gravity = -9.81f;
        private CharacterController _controller;
        protected PlayerInputSO _playerInput;
        protected MyPlayer _player;
        protected EntityAnimator _animator;
        public bool IsAiming { get; private set; }
        public bool IsGround => _controller.isGrounded;
        protected Vector3 _direction;
        protected Vector3 _velocity;
        protected Vector3 _serverPosition;
        protected int _xHash;
        protected int _zHash;
        private float _verticalVelocity;


        public virtual void Initialize(NetworkEntity entity)
        {
            _xHash = Animator.StringToHash("X");
            _zHash = Animator.StringToHash("Z");
            _player = entity as MyPlayer;
            _animator = entity.GetCompo<PlayerAnimator>();
            _controller = entity.GetComponent<CharacterController>();
        }
        private void OnDestroy()
        {
            if (_player.IsOwner)
                _playerInput.OnAimEvent -= HandleAim;
        }
        public void SetPosition(Vector3 position)
        {
            _controller.enabled = false;
            _player.transform.position = position;
            _controller.enabled = true;
        }
        #region SetParameters
        public void HandleAim(bool obj)
            => IsAiming = obj;
        public void SetMovement(Vector2 dir)
            => _direction = new Vector3(dir.x, 0, dir.y);
        public void StopImmediately()
            => _direction = Vector3.zero;
        public void SetServerPosition(Vector3 pos)
            => _serverPosition = pos;
        #endregion
        #region Movement
        protected virtual void FixedUpdate()
        {
            CalculateMovement();
            CalculateRotation();
            ApplyGravity();
            Move();
        }
        protected abstract void CalculateRotation();
        protected abstract void CalculateMovement();
        private void ApplyGravity()
        {
            if (IsGround && _verticalVelocity < 0)
                _verticalVelocity = -0.03f;
            else
                _verticalVelocity += gravity * Time.fixedDeltaTime;
            _velocity.y = _verticalVelocity;
        }
        private void Move()
        {
            _controller.Move(_velocity);
        }
        #endregion
    }
}
