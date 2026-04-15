using UnityEngine;
using Utilities;

namespace Platformer
{
    public class ConeDetectionStrategy : IDetectionStrategy
    {
        private readonly float detectionAngle;
        private readonly float detectionRadius;
        private readonly float innerDetectionRadius;
        
        public ConeDetectionStrategy(float detectionAngle, float detectionRadius, float innerDetectionRadius)
        {
            this.detectionAngle = detectionAngle;
            this.detectionRadius = detectionRadius;
            this.innerDetectionRadius = innerDetectionRadius;
        }
        
        public bool Execute(Transform player, Transform detector, CountdownTimer timer)
        {
            if (timer.IsRunning) return false;

            var directionToPlayer = player.position - detector.position;
            var angleToPlayer = Vector3.Angle(directionToPlayer, detector.forward);

            // 如果玩家不在检测角度 + 外部半径范围内（即敌人前方的锥形区域），
            // 或者不在内部半径范围内，则返回 false
            if ((!(angleToPlayer < detectionAngle / 2f) || !(directionToPlayer.magnitude < detectionRadius)) &&
                !(directionToPlayer.magnitude < innerDetectionRadius))
                return false;
            
            timer.Start();
            return true;
        }
    }
}