using System;
using UnityEngine;
using Utilities; // 假设 CountDownTimer 在这个命名空间下

namespace Platformer
{
    /// <summary>
    /// 可收集物品生成管理器。
    /// 负责按照时间间隔，在预定义的生成点上生成收集品。
    /// </summary>
    public class CollectibleSpawnManager : EntitySpawnManager
    {
        [SerializeField] private CollectibleData[] collectibleData; // 可生成的物品数据配置数组
        [SerializeField] private float spawnInterval = 1f;          // 生成间隔时间（秒）

        private EntitySpawner<Collectible> spawner; // 具体的收集品生成器

        private CountdownTimer spawnTimer;          // 倒计时器，控制生成频率
        private int counter;                        // 当前已生成的数量/生成点索引

        /// <summary>
        /// 初始化。
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            // 初始化生成器，传入工厂（负责创建）和策略（负责选址）
            spawner = new EntitySpawner<Collectible>(new EntityFactory<Collectible>(collectibleData),
                spawnPointStrategy);
            
            // 初始化计时器
            spawnTimer = new CountdownTimer(spawnInterval);
            
            // 订阅计时器结束事件
            spawnTimer.OnTimerStop += () =>
            {
                // 如果已经遍历完所有生成点，则停止计时器并退出
                if (counter >= spawnPoints.Length)
                {
                    spawnTimer.Stop();
                    return;
                }

                // 执行生成
                Spawn();
                // 索引递增
                counter++;
                // 重启计时器，开始下一轮倒计时
                spawnTimer.Start();
            };
        }

        /// <summary>
        /// 游戏开始时启动计时器。
        /// </summary>
        private void Start() => spawnTimer.Start();
        
        /// <summary>
        /// 每一帧更新计时器。
        /// </summary>
        private void Update() => spawnTimer.Tick(Time.deltaTime);

        /// <summary>
        /// 执行生成逻辑。
        /// </summary>
        public override void Spawn() => spawner.Spawn();
    }
}