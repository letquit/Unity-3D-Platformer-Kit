using UnityEngine;
using UnityEngine.AI;

namespace Platformer
{
    /// <summary>
    /// 敌人攻击状态。
    /// 处理敌人进入攻击模式时的行为，包括播放动画、移动和造成伤害。
    /// </summary>
    public class EnemyAttackState : EnemyBaseState
    {
        private readonly NavMeshAgent agent; // 寻路代理
        private readonly Transform player; // 玩家变换组件
        
        public EnemyAttackState(Enemy enemy, Animator animator, NavMeshAgent agent, Transform player) : base(enemy, animator)
        {
            this.agent = agent;
            this.player = player;
        }

        /// <summary>
        /// 当进入此状态时调用。
        /// 播放攻击动画。
        /// </summary>
        public override void OnEnter()
        {
            Debug.Log("Attack");
            animator.CrossFade(AttackHash, crossFadeDuration);
        }

        /// <summary>
        /// 每帧更新逻辑。
        /// 持续向玩家移动并尝试攻击。
        /// </summary>
        public override void Update()
        {
            // 设置寻路目标为玩家当前位置
            agent.SetDestination(player.position);
            // 尝试执行攻击（内部应包含冷却检查）
            enemy.Attack();
        }
    }
}