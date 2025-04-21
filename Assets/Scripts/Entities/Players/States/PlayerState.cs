using Blade.FSM;

namespace Scripts.Entities.Players.States
{
    public class PlayerState : EntityState
    {
        protected Player _player;
        protected PlayerMovement _movement;
        public PlayerState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _player = entity as Player;
            _movement = _player.GetCompo<PlayerMovement>();
        }
    }
}
