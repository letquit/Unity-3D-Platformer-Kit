using System;
using UnityEngine;

namespace Platformer
{
    /// <summary>
    /// 可收集物品类。
    /// 继承自 Entity，处理玩家拾取物品的逻辑。
    /// 使用事件通道解耦分数更新逻辑。
    /// </summary>
    public class Collectible : Entity
    {
        [SerializeField] private int score = 10; // 拾取该物品获得的分数
        [SerializeField] private IntEventChannel scoreChannel; // 分数增加的事件通道

        /// <summary>
        /// 当其他碰撞体进入触发器时调用。
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            // 检查碰撞体是否为玩家
            if (other.CompareTag("Player"))
            {
                // 通过事件通道广播分数增加
                scoreChannel.Invoke(score);
                // 销毁该物品
                Destroy(gameObject);
            }
        }
    }
}