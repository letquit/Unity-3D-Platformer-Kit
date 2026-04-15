using UnityEngine;

namespace Platformer
{
    /// <summary>
    /// 线性生成点策略。
    /// 按照数组顺序依次返回生成点，当到达末尾时自动循环回开头。
    /// 实现了 ISpawnPointStrategy 接口。
    /// </summary>
    public class LinearSpawnPointStrategy : ISpawnPointStrategy
    {
        private int index = 0;          // 当前生成点的索引
        private Transform[] spawnPoints; // 所有生成点的数组

        /// <summary>
        /// 构造函数，注入生成点数组。
        /// </summary>
        public LinearSpawnPointStrategy(Transform[] spawnPoints)
        {
            this.spawnPoints = spawnPoints;
        }
        
        /// <summary>
        /// 获取下一个生成点（线性顺序）。
        /// </summary>
        /// <returns>当前的生成点 Transform</returns>
        public Transform NextSpawnPoint()
        {
            // 获取当前索引对应的生成点
            Transform result = spawnPoints[index];
            
            // 索引递增，并使用取模运算实现循环（0 -> 1 -> ... -> N -> 0）
            index = (index + 1) % spawnPoints.Length;
            
            return result;
        }
    }
}