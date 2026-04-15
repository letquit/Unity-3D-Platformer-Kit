using UnityEngine;
using UnityEngine.AI;

namespace Platformer
{
    /// <summary>
    /// 敌人巡逻状态。
    /// 处理敌人在指定范围内随机移动的逻辑。
    /// </summary>
    public class EnemyWanderState : EnemyBaseState
    {
        private readonly NavMeshAgent agent; // 寻路代理
        private readonly Vector3 startPoint; // 巡逻中心点
        private readonly float wanderRadius; // 巡逻半径
        
        public EnemyWanderState(Enemy enemy, Animator animator, NavMeshAgent agent, float wanderRadius) : base(enemy, animator)
        {
            this.agent = agent;
            this.startPoint = enemy.transform.position;
            this.wanderRadius = wanderRadius;
        }

        /// <summary>
        /// 当进入此状态时调用。
        /// 切换到行走动画。
        /// </summary>
        public override void OnEnter()
        {
            Debug.Log("Wander");
            animator.CrossFade(WalkHash, crossFadeDuration);
        }

        /// <summary>
        /// 每帧更新逻辑。
        /// 检查是否到达目标，若到达则选择新的随机目标点。
        /// </summary>
        public override void Update()
        {
            if (HasReachedDestination())
            {
                // 生成随机方向
                var randomDirection = Random.insideUnitSphere * wanderRadius;
                // 基于起始点计算世界坐标
                randomDirection += startPoint;
                
                NavMeshHit hit;
                // 在NavMesh上采样最近的可行位置
                NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1);
                var finalPosition = hit.position;

                agent.SetDestination(finalPosition);
            }
        }

        /// <summary>
        /// 检查是否已到达目标点。
        /// </summary>
        private bool HasReachedDestination()
        {
            return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance &&
                   (!agent.hasPath || agent.velocity.sqrMagnitude == 0f);
        }
    }
}