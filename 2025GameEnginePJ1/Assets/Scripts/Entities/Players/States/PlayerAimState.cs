using Assets.Scripts.Entities;
using Scripts.Entities.Players.MyPlayers;

namespace Scripts.Entities.Players.States
{
    public abstract class PlayerAimState : PlayerState
    {
        private MyPlayerAttackCompo _attackCompo;
        private EntityAnimatorTrigger _triggerCompo;
        public PlayerAimState(NetworkEntity entity, int animationHash) : base(entity, animationHash)
        {
            _attackCompo = entity.GetCompo<MyPlayerAttackCompo>();
            _triggerCompo = _player.GetCompo<EntityAnimatorTrigger>();
        }
        public override void Enter()
        {
            base.Enter();
            _player.PlayerInput.OnAttackEvent += _attackCompo.HandleAttack;
            _player.PlayerInput.OnAimEvent += HandleAim;
        }

        public override void Exit()
        {
            _player.PlayerInput.OnAttackEvent -= _attackCompo.HandleAttack;
            _player.PlayerInput.OnAimEvent -= HandleAim;
            base.Exit();
        }
        private void HandleAim(bool obj)
        {
            if (obj == false)
            {
                _attackCompo.HandleAttack(false);
                _player.ChangeState("Idle");
            }
        }

    }
}
