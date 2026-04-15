using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Platformer
{
    /// <summary>
    /// 随机生成点策略。
    /// 随机选择生成点，但保证在一次完整遍历中不重复，直到所有点都被使用过。
    /// 实现了 ISpawnPointStrategy 接口。
    /// </summary>
    public class RandomSpawnPointStrategy : ISpawnPointStrategy
    {
        private List<Transform> unusedSpawnPoints; // 当前轮次尚未使用的生成点列表
        private Transform[] spawnPoints;           // 所有原始生成点的数组

        /// <summary>
        /// 构造函数，注入生成点数组并初始化列表。
        /// </summary>
        public RandomSpawnPointStrategy(Transform[] spawnPoints)
        {
            this.spawnPoints = spawnPoints;
            // 将数组复制到列表中，以便进行移除操作
            unusedSpawnPoints = new List<Transform>(spawnPoints);
        }
        
        /// <summary>
        /// 获取下一个生成点（不放回随机）。
        /// </summary>
        /// <returns>选中的生成点 Transform</returns>
        public Transform NextSpawnPoint()
        {
            // 如果所有点都已使用过，重置列表，开始新一轮
            if (!unusedSpawnPoints.Any())
            {
                unusedSpawnPoints = new List<Transform>(spawnPoints);
            }

            // 从剩余点中随机选择一个索引
            var randomIndex = Random.Range(0, unusedSpawnPoints.Count);
            
            // 获取该点
            Transform result = unusedSpawnPoints[randomIndex];
            
            // 从列表中移除该点，防止本轮再次选中
            unusedSpawnPoints.RemoveAt(randomIndex);
            
            return result;
        }
    }
}