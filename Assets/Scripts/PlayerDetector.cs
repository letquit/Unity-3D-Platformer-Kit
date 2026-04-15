using System;
using UnityEngine;
using Utilities;

namespace Platformer
{
    /// <summary>
    /// 玩家检测器。
    /// 负责判断敌人是否能检测到玩家，并管理检测策略。
    /// </summary>
    public class PlayerDetector : MonoBehaviour
    {
        [SerializeField] private float detectionAngle = 60f;    // 前方锥形视野角度
        [SerializeField] private float detectionRadius = 10f;   // 最大检测半径（外圈）
        [SerializeField] private float innerDetectionRadius = 5f;   // 内部检测半径（内圈，通常用于立即发现）
        [SerializeField] private float detectionCooldown = 1f;  // 检测冷却时间（秒）
        [SerializeField] private float attackRange = 2f;    // 攻击范围
        
        public Transform Player { get; private set; } // 玩家变换引用
        public Health PlayerHealth { get; private set; } // 玩家生命值引用
        
        private CountdownTimer detectionTimer; // 检测冷却计时器
        
        private IDetectionStrategy detectionStrategy; // 具体的检测策略（如锥形检测）

        private void Awake()
        {
            // 查找玩家对象并获取组件
            Player = GameObject.FindGameObjectWithTag("Player").transform;
            PlayerHealth = Player.GetComponent<Health>();
        }

        private void Start()
        {
            detectionTimer = new CountdownTimer(detectionCooldown);
            // 初始化具体的检测策略
            detectionStrategy = new ConeDetectionStrategy(detectionAngle, detectionRadius, innerDetectionRadius);
        }

        private void Update() => detectionTimer.Tick(Time.deltaTime);

        /// <summary>
        /// 判断是否可以检测到玩家。
        /// </summary>
        /// <returns>如果检测到玩家返回 true，否则返回 false</returns>
        public bool CanDetectPlayer()
        {
            // 如果计时器正在运行（冷却中），则无法检测
            // 否则执行具体的检测策略
            return detectionTimer.IsRunning || detectionStrategy.Execute(Player, transform, detectionTimer);
        }

        /// <summary>
        /// 判断玩家是否在攻击范围内。
        /// </summary>
        /// <returns>如果在攻击范围内返回 true</returns>
        public bool CanAttackPlayer()
        {
            var directionToPlayer = Player.position - transform.position;
            return directionToPlayer.magnitude <= attackRange;
        }
        
        /// <summary>
        /// 允许外部动态切换检测策略。
        /// </summary>
        public void SetDetectionStrategy(IDetectionStrategy detectionStrategy) => this.detectionStrategy = detectionStrategy;

        /// <summary>
        /// 在编辑器中绘制检测范围的可视化辅助线。
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            
            // 绘制检测半径的线框球体
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
            Gizmos.DrawWireSphere(transform.position, innerDetectionRadius);
            
            // 计算锥形视野的左右边界方向
            Vector3 forwardConeDirection = Quaternion.Euler(0, detectionAngle / 2, 0) * transform.forward * detectionRadius;
            Vector3 backwardConeDirection =
                Quaternion.Euler(0, -detectionAngle / 2, 0) * transform.forward * detectionRadius;
            
            // 绘制锥形边界线
            Gizmos.DrawLine(transform.position, transform.position + forwardConeDirection);
            Gizmos.DrawLine(transform.position, transform.position + backwardConeDirection);
        }
    }
}