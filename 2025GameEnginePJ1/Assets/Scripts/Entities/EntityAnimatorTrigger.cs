using Scripts.Entities;
using System;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class EntityAnimatorTrigger : MonoBehaviour, IEntityComponent
    {
        public event Action OnAnimationEndTrigger;
        public event Action OnAttackTrigger;
        public void Initialize(NetworkEntity entity)
        {
        }
        public void AnimationEnd()
        {
            OnAnimationEndTrigger?.Invoke();
        }
        public void AttackTrigger()
        {
            OnAttackTrigger?.Invoke();
        }
    }
}
