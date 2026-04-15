using System;
using UnityEngine;

namespace Platformer
{
    /// <summary>
    /// 游戏管理器。
    /// 使用单例模式管理全局游戏状态，如分数。
    /// 确保整个游戏中只有一个实例存在。
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        // 单例实例
        public static GameManager Instance { get; private set; }
        
        // 当前分数（只读，外部只能通过方法修改）
        public int Score { get; private set; }

        private void Awake()
        {
            // 如果当前没有实例，则注册此实例
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                // 如果已有实例，则销毁当前对象，防止重复
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// 增加分数。
        /// </summary>
        /// <param name="score">要增加的分数值</param>
        public void AddScore(int score)
        {
            Score += score;
        }
    }
}