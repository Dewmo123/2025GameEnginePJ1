using Assets.Scripts.Entities;
using System;
using UnityEngine;

namespace Scripts.Entities.Players.States
{
    public abstract class PlayerAimState : PlayerState
    {
        private PlayerAttackComponent _attackCompo;
        private EntityAnimatorTrigger _triggerCompo;
        public PlayerAimState(NetworkEntity entity, int animationHash) : base(entity, animationHash)
        {
            _attackCompo = entity.GetCompo<PlayerAttackComponent>();
            _triggerCompo = _player.GetCompo<EntityAnimatorTrigger>();
        }
        public override void Enter()
        {
            base.Enter();
            _player.PlayerInput.OnAttackEvent += _attackCompo.HandleAttack;
            _player.PlayerInput.OnAimEvent += HandleAim;
            _triggerCompo.OnAttackTrigger += HandleAttack;
        }

        public override void Exit()
        {
            _triggerCompo.OnAttackTrigger -= HandleAttack;
            _player.PlayerInput.OnAttackEvent -= _attackCompo.HandleAttack;
            _player.PlayerInput.OnAimEvent -= HandleAim;
            base.Exit();
        }
        private void HandleAttack()
        {
            _attackCompo.ShootBullet();
        }
        private void HandleAim(bool obj)
        {
            if (obj == false)
                _player.ChangeState("Idle");
        }

    }
}
