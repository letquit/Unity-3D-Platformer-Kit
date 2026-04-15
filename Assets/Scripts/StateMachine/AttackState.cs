using UnityEngine;

namespace Platformer
{
    /// <summary>
    /// 攻击状态。
    /// 处理玩家角色的攻击行为，包括播放动画和执行攻击逻辑。
    /// </summary>
    public class AttackState : BaseState
    {
        // 假设 AttackHash 在基类或此类中定义为 static readonly int
        // private static readonly int AttackHash = Animator.StringToHash("Attack");
        // private const float crossFadeDuration = 0.1f;

        public AttackState(PlayerController player, Animator animator) : base(player, animator)
        {
        }
        
        /// <summary>
        /// 当进入此状态时调用。
        /// 负责播放攻击动画并触发攻击动作。
        /// </summary>
        public override void OnEnter()
        {
            // 平滑过渡到攻击动画
            animator.CrossFade(AttackHash, crossFadeDuration);
            // 执行攻击逻辑（如造成伤害、播放音效等）
            player.Attack();
        }
        
        /// <summary>
        /// 物理更新循环。
        /// 在攻击过程中依然允许玩家移动。
        /// </summary>
        public override void FixedUpdate()
        {
            player.HandleMovement();
        }
    }
}