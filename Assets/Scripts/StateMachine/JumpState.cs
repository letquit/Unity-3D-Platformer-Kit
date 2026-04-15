using UnityEngine;

namespace Platformer
{
    /// <summary>
    /// 跳跃状态。
    /// 处理玩家角色的跳跃行为，包括播放动画、处理跳跃物理和维持水平移动。
    /// </summary>
    public class JumpState : BaseState
    {
        public JumpState(PlayerController player, Animator animator) : base(player, animator)
        {
        }
        
        /// <summary>
        /// 当进入此状态时调用。
        /// 负责播放跳跃动画。
        /// </summary>
        public override void OnEnter()
        {
            // 平滑过渡到跳跃动画
            animator.CrossFade(JumpHash, crossFadeDuration);
        }
        
        /// <summary>
        /// 物理更新循环。
        /// 在跳跃过程中处理垂直方向的物理和水平方向的移动控制。
        /// </summary>
        public override void FixedUpdate()
        {
            // 处理跳跃相关的物理逻辑（如施加重力）
            player.HandleJump();
            // 处理水平移动逻辑（空中控制）
            player.HandleMovement();
        }
    }
}