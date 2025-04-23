namespace Scripts.Entities.Players.States
{
    public class PlayerIdleState : PlayerState
    {
        public PlayerIdleState(NetworkEntity entity, int animationHash) : base(entity, animationHash)
        {
        }
        public override void Enter()
        {
            base.Enter();
            _movement.StopImmediately();
        }
    }
}
