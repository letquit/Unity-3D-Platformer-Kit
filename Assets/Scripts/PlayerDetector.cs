using System;
using UnityEngine;
using Utilities;

namespace Platformer
{
    public class PlayerDetector : MonoBehaviour
    {
        [SerializeField] private float detectionAngle = 60f;    // 敌人前方的锥形区域
        [SerializeField] private float detectionRadius = 10f;   // 敌人周围的大圆圈
        [SerializeField] private float innerDetectionRadius = 5f;   // 敌人周围的小圆圈
        [SerializeField] private float detectionCooldown = 1f;  // 两次检测之间的时间间隔
        [SerializeField] private float attackRange = 2f;    // 敌人攻击玩家的距离
        
        public Transform Player { get; private set; }
        private CountdownTimer detectionTimer;
        
        private IDetectionStrategy detectionStrategy;

        private void Start()
        {
            detectionTimer = new CountdownTimer(detectionCooldown);
            Player = GameObject.FindGameObjectWithTag("Player").transform;
            detectionStrategy = new ConeDetectionStrategy(detectionAngle, detectionRadius, innerDetectionRadius);
        }

        private void Update() => detectionTimer.Tick(Time.deltaTime);

        public bool CanDetectPlayer()
        {
            return detectionTimer.IsRunning || detectionStrategy.Execute(Player, transform, detectionTimer);
        }

        public bool CanAttackPlayer()
        {
            var directionToPlayer = Player.position - transform.position;
            return directionToPlayer.magnitude <= attackRange;
        }
        
        public void SetDetectionStrategy(IDetectionStrategy detectionStrategy) => this.detectionStrategy = detectionStrategy;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            
            // 绘制半径的球体线框
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
            Gizmos.DrawWireSphere(transform.position, innerDetectionRadius);
            
            // 计算我们的锥形方向
            Vector3 forwardConeDirection = Quaternion.Euler(0, detectionAngle / 2, 0) * transform.forward * detectionRadius;
            Vector3 backwardConeDirection =
                Quaternion.Euler(0, -detectionAngle / 2, 0) * transform.forward * detectionRadius;
            
            // 绘制线条来表示锥形
            Gizmos.DrawLine(transform.position, transform.position + forwardConeDirection);
            Gizmos.DrawLine(transform.position, transform.position + backwardConeDirection);
        }
    }
}