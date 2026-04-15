using UnityEngine;

namespace Platformer
{
    /// <summary>
    /// 移动状态。
    /// 处理玩家角色的正常移动行为（如行走或奔跑）。
    /// </summary>
    public class LocomotionState : BaseState
    {
        public LocomotionState(PlayerController player, Animator animator) : base(player, animator)
        {
        }
        
        /// <summary>
        /// 当进入此状态时调用。
        /// 负责播放移动动画。
        /// </summary>
        public override void OnEnter()
        {
            // 平滑过渡到移动动画
            animator.CrossFade(LocomotionHash, crossFadeDuration);
        }
        
        /// <summary>
        /// 物理更新循环。
        /// 处理角色的移动逻辑。
        /// </summary>
        public override void FixedUpdate()
        {
            player.HandleMovement();
        }
    }
}