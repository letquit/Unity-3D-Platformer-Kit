using UnityEngine;
using UnityEngine.AI;

namespace Platformer
{
    /// <summary>
    /// 敌人追逐状态。
    /// 处理敌人发现玩家后的追击行为。
    /// </summary>
    public class EnemyChaseState : EnemyBaseState
    {
        private readonly NavMeshAgent agent; // 寻路代理
        private readonly Transform player; // 玩家变换组件
        
        public EnemyChaseState(Enemy enemy, Animator animator, NavMeshAgent agent, Transform player) : base(enemy, animator)
        {
            this.agent = agent;
            this.player = player;
        }

        /// <summary>
        /// 当进入此状态时调用。
        /// 切换到奔跑动画。
        /// </summary>
        public override void OnEnter()
        {
            Debug.Log("Chase");
            animator.CrossFade(RunHash, crossFadeDuration);
        }

        /// <summary>
        /// 每帧更新逻辑。
        /// 持续更新寻路目标为玩家位置。
        /// </summary>
        public override void Update()
        {
            agent.SetDestination(player.position);
        }
    }
}