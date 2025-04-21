using UnityEngine;

namespace Scripts.Entities.Players.States
{
    public class PlayerAimWalkState : PlayerAimState
    {
        private int _xHash;
        private int _zHash;
        public PlayerAimWalkState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _xHash = Animator.StringToHash("X");
            _zHash = Animator.StringToHash("Z");
        }
        public override void Update()
        {
            base.Update();
            Vector2 moveVec = _player.PlayerInput.MovementKey;
            if (moveVec.magnitude == 0)
                _player.ChangeState("AimIdle");
            _entityAnimator.SetParam(_xHash, moveVec.x);
            _entityAnimator.SetParam(_zHash, moveVec.y);
        }
    }
}
