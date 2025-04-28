namespace Scripts.Entities.Players.States
{
    public class PlayerIdleState : PlayerState
    {
        public PlayerIdleState(NetworkEntity entity, int animationHash) : base(entity, animationHash)
        {
        }
        public override void Update()
        {
            base.Update();
            if (_player.PlayerInput.MovementKey.magnitude > 0.1f)
                _player.ChangeState("Walk");
        }
        public override void Enter()
        {
            base.Enter();
            _movement.StopImmediately();
        }
    }
}
