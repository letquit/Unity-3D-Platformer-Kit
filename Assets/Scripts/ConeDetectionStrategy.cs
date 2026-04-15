using UnityEngine;
using Utilities;

namespace Platformer
{
    /// <summary>
    /// 锥形检测策略。
    /// 实现 IDetectionStrategy 接口，用于检测玩家是否进入敌人的锥形视野或近距离范围。
    /// </summary>
    public class ConeDetectionStrategy : IDetectionStrategy
    {
        private readonly float detectionAngle; // 视野角度（总角度）
        private readonly float detectionRadius; // 外部检测半径
        private readonly float innerDetectionRadius; // 内部（近身）检测半径
        
        public ConeDetectionStrategy(float detectionAngle, float detectionRadius, float innerDetectionRadius)
        {
            this.detectionAngle = detectionAngle;
            this.detectionRadius = detectionRadius;
            this.innerDetectionRadius = innerDetectionRadius;
        }
        
        /// <summary>
        /// 执行检测逻辑。
        /// </summary>
        /// <param name="player">玩家变换组件</param>
        /// <param name="detector">探测器（敌人）变换组件</param>
        /// <param name="timer">检测冷却计时器</param>
        /// <returns>如果检测到玩家则返回 true，否则返回 false</returns>
        public bool Execute(Transform player, Transform detector, CountdownTimer timer)
        {
            // 如果计时器正在运行（处于冷却中），则不进行检测
            if (timer.IsRunning) return false;

            // 计算指向玩家的向量
            var directionToPlayer = player.position - detector.position;
            // 计算玩家相对于探测器前方的角度
            var angleToPlayer = Vector3.Angle(directionToPlayer, detector.forward);

            // 检测逻辑：
            // 如果玩家不在检测角度 + 外部半径范围内（即敌人前方的锥形区域），
            // 且 也不在内部半径范围内（近身盲区），则返回 false
            if ((!(angleToPlayer < detectionAngle / 2f) || !(directionToPlayer.magnitude < detectionRadius)) &&
                !(directionToPlayer.magnitude < innerDetectionRadius))
                return false;
            
            // 检测通过，启动计时器（进入警戒或攻击状态）
            timer.Start();
            return true;
        }
    }
}