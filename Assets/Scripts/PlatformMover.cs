using System;
using DG.Tweening; // 引入 DOTween 插件
using UnityEngine;

namespace Platformer
{
    /// <summary>
    /// 平台移动器。
    /// 使用 DOTween 实现平台在两点之间的平滑往复移动。
    /// </summary>
    public class PlatformMover : MonoBehaviour
    {
        [SerializeField] private Vector3 moveTo = Vector3.zero; // 目标相对位移
        [SerializeField] private float moveTime = 1f;           // 移动耗时
        [SerializeField] private Ease ease = Ease.InOutQuad;    // 缓动类型

        private Vector3 startPosition; // 初始位置

        private void Start()
        {
            startPosition = transform.position;
            Move();
        }

        /// <summary>
        /// 执行移动动画。
        /// </summary>
        private void Move()
        {
            // DOMove: 移动 Transform 到指定位置
            // SetEase: 设置加减速曲线
            // SetLoops: 设置循环次数 (-1 为无限) 和模式 (Yoyo 为往复)
            transform.DOMove(startPosition + moveTo, moveTime)
                .SetEase(ease)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}