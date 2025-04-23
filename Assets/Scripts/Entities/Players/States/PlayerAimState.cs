using System;
using UnityEngine;

namespace Scripts.Entities.Players.States
{
    public abstract class PlayerAimState : PlayerState
    {
        public PlayerAimState(NetworkEntity entity, int animationHash) : base(entity, animationHash)
        {
        }
        public override void Enter()
        {
            base.Enter();
            _player.PlayerInput.OnAimEvent += HandleAim;
        }
        public override void Exit()
        {
            _player.PlayerInput.OnAimEvent -= HandleAim;
            base.Exit();
        }
        private void HandleAim(bool obj)
        {
            if (obj == false)
                _player.ChangeState("Idle");
        }
    }
}
