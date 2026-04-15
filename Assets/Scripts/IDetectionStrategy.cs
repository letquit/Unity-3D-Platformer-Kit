using UnityEngine;
using Utilities;

namespace Platformer
{
    /// <summary>
    /// 检测策略接口。
    /// 定义了检测玩家的具体算法契约。
    /// 使用策略模式，允许在运行时切换不同的检测逻辑（如锥形检测、圆形检测等）。
    /// </summary>
    public interface IDetectionStrategy
    {
        /// <summary>
        /// 执行检测逻辑。
        /// </summary>
        /// <param name="player">玩家变换组件</param>
        /// <param name="detector">探测器（如敌人）变换组件</param>
        /// <param name="timer">用于控制检测频率的计时器</param>
        /// <returns>如果检测到玩家则返回 true，否则返回 false</returns>
        bool Execute(Transform player, Transform detector, CountdownTimer timer);
    }
}