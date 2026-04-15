using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Platformer
{
    /// <summary>
    /// 分数UI控制器。
    /// 负责将游戏管理器中的分数同步显示到屏幕上。
    /// </summary>
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText; // 分数文本组件

        private void Start()
        {
            UpdateScore();
        }

        /// <summary>
        /// 请求更新分数显示。
        /// 使用协程延迟到下一帧更新，以确保获取最新的数据。
        /// </summary>
        public void UpdateScore()
        {
            StartCoroutine(UpdateScoreNextFrame());
        }

        /// <summary>
        /// 协程：等待一帧后更新文本。
        /// </summary>
        private IEnumerator UpdateScoreNextFrame()
        {
            // 等待当前帧结束，在下一帧更新UI
            yield return null;
            scoreText.text = GameManager.Instance.Score.ToString();
        }
    }
}