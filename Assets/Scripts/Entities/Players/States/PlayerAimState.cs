using System;

namespace Scripts.Entities.Players.States
{
    public abstract class PlayerAimState : PlayerState
    {
        public PlayerAimState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }
        public override void Enter()
        {
            base.Enter();
            _player.PlayerInput.OnAimEvent += HandleAim;
        }
        public override void Update()
        {
            base.Update();
            _movement.SetDirection(_player.PlayerInput.GetWorldPosition());
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
