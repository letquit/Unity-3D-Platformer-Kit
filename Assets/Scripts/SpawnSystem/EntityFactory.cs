using UnityEngine;

namespace Platformer
{
    /// <summary>
    /// 实体工厂。
    /// 负责根据数据配置数组，随机实例化具体的实体对象。
    /// 遵循工厂模式。
    /// </summary>
    /// <typeparam name="T">要创建的实体类型，必须继承自 Entity</typeparam>
    public class EntityFactory<T> : IEntityFactory<T> where T : Entity
    {
        private EntityData[] data; // 实体数据配置数组（如：金币数据、宝石数据）

        /// <summary>
        /// 构造函数，注入数据配置。
        /// </summary>
        public EntityFactory(EntityData[] data)
        {
            this.data = data;
        }
        
        /// <summary>
        /// 创建一个实体实例。
        /// </summary>
        /// <param name="spawnPoint">生成的位置参考点</param>
        /// <returns>生成的实体组件</returns>
        public T Create(Transform spawnPoint)
        {
            // 从数据数组中随机选择一个配置
            EntityData entityData = data[Random.Range(0, data.Length)];
            
            // 实例化预制体
            GameObject instance = GameObject.Instantiate(entityData.prefab, spawnPoint.position, spawnPoint.rotation);
            
            // 返回挂载在实例上的组件
            return instance.GetComponent<T>();
        }
    }
}