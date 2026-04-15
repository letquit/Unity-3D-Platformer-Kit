using UnityEngine;

namespace Platformer
{
    /// <summary>
    /// 可收集物品的数据配置。
    /// 继承自 EntityData，用于定义收集品（如金币、宝石）的属性。
    /// 可以在右键菜单中创建：Assets -> Create -> Platformer -> Collectible Data
    /// </summary>
    [CreateAssetMenu(fileName = "CollectibleData", menuName = "Platformer/Collectible Data")]
    public class CollectibleData : EntityData
    {
        [Tooltip("拾取该物品后获得的分数")]
        public int score;
    }
}