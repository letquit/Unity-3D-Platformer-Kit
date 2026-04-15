using System;
using DG.Tweening;
using UnityEngine;

namespace Platformer
{
    /// <summary>
    /// 生成特效控制器。
    /// 处理物体生成时的缩放动画、粒子特效和音效。
    /// </summary>
    [RequireComponent(typeof(AudioSource))] // 确保挂载此脚本时自动添加 AudioSource 组件
    public class SpawnEffects : MonoBehaviour
    {
        [SerializeField] private GameObject spawnVFX; // 生成时播放的粒子特效预制体
        [SerializeField] private float animationDuration = 1f; // 缩放动画持续时间

        private void Start()
        {
            // 初始状态：将物体缩放设为 0（不可见）
            transform.localScale = Vector3.zero;
            
            // 播放缩放动画：从 0 放大到 1 (正常大小)
            // SetEase(Ease.OutBack): 使用回弹缓动，产生“弹跳”效果
            transform.DOScale(Vector3.one, animationDuration).SetEase(Ease.OutBack);

            // 如果指定了特效预制体，则在当前位置实例化它
            if (spawnVFX != null)
            {
                Instantiate(spawnVFX, transform.position, Quaternion.identity);
            }
            
            // 播放生成音效
            GetComponent<AudioSource>().Play();
        }
    }
}