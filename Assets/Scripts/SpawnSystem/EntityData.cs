using UnityEngine;

namespace Platformer
{
    /// <summary>
    /// 实体数据基类。
    /// 用于定义游戏实体的通用数据（如预制体引用）。
    /// 其他具体的数据类（如 CollectibleData, EnemyData）应继承此类。
    /// </summary>
    public class EntityData : ScriptableObject
    {
        [Tooltip("该实体对应的预制体")]
        public GameObject prefab;
    }
}