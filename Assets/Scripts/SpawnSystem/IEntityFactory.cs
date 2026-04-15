using Unity.VisualScripting.FullSerializer; // 看起来可能是一个多余的引用，或者用于特定的序列化需求
using UnityEngine;

namespace Platformer
{
    /// <summary>
    /// 实体工厂接口。
    /// 定义了创建实体的标准契约。
    /// 任何实现此接口的类都必须能够根据生成点创建具体的实体。
    /// </summary>
    /// <typeparam name="T">要创建的实体类型，必须继承自 Entity</typeparam>
    public interface IEntityFactory<T> where T : Entity
    {
        /// <summary>
        /// 创建一个实体实例。
        /// </summary>
        /// <param name="spawnPoint">生成的位置参考点</param>
        /// <returns>生成的实体组件</returns>
        T Create(Transform spawnPoint);
    }
}