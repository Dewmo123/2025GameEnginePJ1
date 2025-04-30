using Scripts.Entities;
using System;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class EntityAnimatorTrigger : MonoBehaviour, IEntityComponent
    {
        public event Action OnAnimationEndTrigger;
        public void Initialize(NetworkEntity entity)
        {
        }
        public void AnimationEnd()
        {
            OnAnimationEndTrigger?.Invoke();
        }
    }
}
