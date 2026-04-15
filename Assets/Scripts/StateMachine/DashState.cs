using UnityEngine;

namespace Platformer
{
    /// <summary>
    /// 冲刺状态。
    /// 处理玩家角色的冲刺行为，包括播放动画和维持移动逻辑。
    /// </summary>
    public class DashState : BaseState
    {
        public DashState(PlayerController player, Animator animator) : base(player, animator)
        {
        }
        
        /// <summary>
        /// 当进入此状态时调用。
        /// 负责播放冲刺动画。
        /// </summary>
        public override void OnEnter()
        {
            // 平滑过渡到冲刺动画
            animator.CrossFade(DashHash, crossFadeDuration);
        }
        
        /// <summary>
        /// 物理更新循环。
        /// 在冲刺过程中依然允许玩家移动。
        /// </summary>
        public override void FixedUpdate()
        {
            player.HandleMovement();
        }
    }
}