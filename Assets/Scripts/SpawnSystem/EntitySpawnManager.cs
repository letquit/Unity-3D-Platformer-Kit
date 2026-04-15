using System;
using UnityEngine;

namespace Platformer
{
    /// <summary>
    /// 实体生成管理器基类。
    /// 负责管理生成点策略，并为子类提供通用的生成框架。
    /// </summary>
    public abstract class EntitySpawnManager : MonoBehaviour
    {
        [Tooltip("选择生成点的策略：线性顺序或随机")]
        [SerializeField] protected SpawnPointStrategyType spawnPointStrategyType = SpawnPointStrategyType.Linear;
        
        [Tooltip("所有可能的生成点位置")]
        [SerializeField] protected Transform[] spawnPoints;
        
        protected ISpawnPointStrategy spawnPointStrategy; // 当前使用的生成策略接口
            
        /// <summary>
        /// 生成点策略类型枚举。
        /// </summary>
        protected enum SpawnPointStrategyType
        {
            Linear, // 按数组顺序依次生成
            Random  // 随机选择生成点
        }

        /// <summary>
        /// 初始化策略。
        /// </summary>
        protected virtual void Awake()
        {
            // 根据枚举选择，实例化具体的策略实现
            spawnPointStrategy = spawnPointStrategyType switch
            {
                SpawnPointStrategyType.Linear => new LinearSpawnPointStrategy(spawnPoints),
                SpawnPointStrategyType.Random => new RandomSpawnPointStrategy(spawnPoints),
                _ => spawnPointStrategy // 默认情况（理论上不会执行，因为枚举已限制）
            };
        }
        
        /// <summary>
        /// 抽象生成方法。
        /// 子类必须实现具体的生成逻辑（如：定时生成、触发生成等）。
        /// </summary>
        public abstract void Spawn();
    }
}