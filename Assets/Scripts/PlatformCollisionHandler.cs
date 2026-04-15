using System;
using UnityEngine;

namespace Platformer
{
    /// <summary>
    /// 平台碰撞处理器。
    /// 处理玩家与移动平台的交互，通过设置父子关系实现随动效果。
    /// </summary>
    public class PlatformCollisionHandler : MonoBehaviour
    {
        private Transform platform; // 当前依附的平台

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("MovingPlatform"))
            {
                // 获取第一个接触点
                ContactPoint contact = other.GetContact(0);
                
                // 检查法线方向，确保是站在平台上方（法线Y轴接近1）
                // 如果法线Y小于0.5，说明是侧面碰撞或斜坡过陡，不吸附
                if (contact.normal.y < 0.5f) return;

                platform = other.transform;
                // 将玩家设为平台的子对象，使其随平台移动
                transform.SetParent(platform);
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.CompareTag("MovingPlatform"))
            {
                // 离开平台时解除父子关系
                transform.SetParent(null);
                platform = null;
            }
        }
    }
}