using System;
using UnityEngine;

namespace Platformer
{
    /// <summary>
    /// 生命值组件。
    /// 管理对象的健康状态，处理伤害并广播生命值百分比。
    /// </summary>
    public class Health : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 100; // 最大生命值
        [SerializeField] private FloatEventChannel playerHealthChannel; // 生命值变化事件通道

        private int currentHealth; // 当前生命值
        
        // 是否死亡
        public bool IsDead => currentHealth <= 0;

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        private void Start()
        {
            PublishHealthPercentage();
        }

        /// <summary>
        /// 受到伤害。
        /// </summary>
        /// <param name="damage">伤害值</param>
        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            PublishHealthPercentage();
        }

        /// <summary>
        /// 发布当前生命值百分比。
        /// </summary>
        private void PublishHealthPercentage()
        {
            if (playerHealthChannel != null)
                // 计算百分比 (0.0 - 1.0)
                playerHealthChannel.Invoke(currentHealth / (float)maxHealth);
        }
    }
}