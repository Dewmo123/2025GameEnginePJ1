using Blade.FSM;
using Scripts.Core.GameSystem;
using System;
using UnityEngine;

namespace Scripts.Entities.Players
{
    public class Player : Entity
    {
        [field: SerializeField] public PlayerInputSO PlayerInput { get; private set; }
        [SerializeField] private StateDataSO[] states;

        private EntityStateMachine _stateMachine;
        protected override void Awake()
        {
            base.Awake();
            _stateMachine = new EntityStateMachine(this, states);
            ChangeState("Idle");
            PlayerInput.OnAimEvent += HandleAim;
        }
        private void Update()
        {
            _stateMachine.UpdateStateMachine();
        }
        private void HandleAim(bool obj)
        {
            if (obj)
                ChangeState("AimIdle");
        }

        public void ChangeState(string newStateName) => _stateMachine.ChangeState(newStateName);
    }
}
