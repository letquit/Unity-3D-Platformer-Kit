namespace Platformer
{
    /// <summary>
    /// 实体生成器。
    /// 负责协调工厂和策略来生成实体。
    /// 它将“创建什么”（工厂）和“在哪里创建”（策略）分离开来。
    /// </summary>
    /// <typeparam name="T">要生成的实体类型，必须继承自 Entity</typeparam>
    public class EntitySpawner<T> where T : Entity
    {
        private IEntityFactory<T> entityFactory;       // 负责创建实体的工厂
        private ISpawnPointStrategy spawnPointStrategy; // 负责决定生成位置的策略

        /// <summary>
        /// 构造函数，注入依赖项。
        /// </summary>
        public EntitySpawner(IEntityFactory<T> entityFactory, ISpawnPointStrategy spawnPointStrategy)
        {
            this.entityFactory = entityFactory;
            this.spawnPointStrategy = spawnPointStrategy;
        }
        
        /// <summary>
        /// 执行生成操作。
        /// 1. 从策略获取下一个生成点。
        /// 2. 命令工厂在该点创建实体。
        /// </summary>
        /// <returns>生成的实体实例</returns>
        public T Spawn()
        {
            return entityFactory.Create(spawnPointStrategy.NextSpawnPoint());
        }
    }
}