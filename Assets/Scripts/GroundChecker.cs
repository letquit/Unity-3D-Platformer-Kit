using System;
using UnityEngine;

namespace Platformer
{
    /// <summary>
    /// 地面检测器。
    /// 使用球形射线检测角色是否接触地面。
    /// </summary>
    public class GroundChecker : MonoBehaviour
    {
        [SerializeField] private float groundDistance = 0.08f; // 检测距离和球体半径
        [SerializeField] private LayerMask groundLayers; // 地面层级掩码
        
        // 是否在地面上（只读）
        public bool IsGrounded { get; private set; }

        private void Update()
        {
            // 从当前位置向下发射球形射线
            // 参数说明：
            // origin: 起点
            // radius: 球体半径
            // direction: 方向（向下）
            // hit: 命中信息（这里用 _ 忽略）
            // maxDistance: 最大距离
            // layerMask: 层级掩码
            IsGrounded = Physics.SphereCast(transform.position, groundDistance, Vector3.down, out _, groundDistance,
                groundLayers);
        }
    }
}