using Assets.Scripts.Entities;
using UnityEngine;

namespace Scripts.Entities.Players
{
    public class PlayerAnimator : EntityAnimator
    {
        public Transform leftHandTarget;
        private void OnAnimatorIK(int layerIndex)
        {

            // 왼손 IK 설정
            if (leftHandTarget != null)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);
            }
        }
    }
}
