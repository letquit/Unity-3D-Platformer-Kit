using System;
using System.Collections;
using Cinemachine;
using KBCore.Refs;
using UnityEngine;

namespace Platformer
{
    /// <summary>
    /// 摄像机管理器。
    /// 负责处理玩家输入与 Cinemachine 虚拟摄像机之间的交互，
    /// 包括鼠标控制、光标锁定和输入平滑处理。
    /// </summary>
    public class CameraManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField, Anywhere] private InputReader input; // 输入读取器
        [SerializeField, Anywhere] private CinemachineFreeLook freeLookVCam; // 自由视角虚拟摄像机

        [Header("Settings")]
        [SerializeField, Range(0.5f, 3f)] private float speedMultiplier = 1f; // 摄像机移动速度倍率

        private bool isRMBPressed; // 鼠标右键是否按下
        private bool cameraMovementLock; // 摄像机移动锁定标志

        private void OnEnable()
        {
            // 订阅输入事件
            input.Look += OnLook;
            input.EnableMouseControlCamera += OnEnableMouseControlCamera;
            input.DisableMouseControlCamera += OnDisableMouseControlCamera;
        }

        private void OnDisable()
        {
            // 取消订阅输入事件
            input.Look -= OnLook;
            input.EnableMouseControlCamera -= OnEnableMouseControlCamera;
            input.DisableMouseControlCamera -= OnDisableMouseControlCamera;
        }

        /// <summary>
        /// 处理摄像机视角移动输入。
        /// </summary>
        /// <param name="cameraMovement">移动向量</param>
        /// <param name="isDeviceMouse">输入设备是否为鼠标</param>
        private void OnLook(Vector2 cameraMovement, bool isDeviceMouse)
        {
            if (cameraMovementLock) return; // 如果锁定则忽略输入
            
            // 如果是鼠标输入但右键未按下，则忽略（仅允许右键拖拽视角）
            if (isDeviceMouse && !isRMBPressed) return;
            
            // 根据输入设备类型选择时间增量，以匹配输入源的更新频率
            float deviceMultiplier = isDeviceMouse ? Time.fixedDeltaTime : Time.deltaTime;
            
            // 直接设置 Cinemachine 自由视角的输入轴值
            freeLookVCam.m_XAxis.m_InputAxisValue = cameraMovement.x * speedMultiplier * deviceMultiplier;
            freeLookVCam.m_YAxis.m_InputAxisValue = cameraMovement.y * speedMultiplier * deviceMultiplier;
        }

        /// <summary>
        /// 启用鼠标控制摄像机模式。
        /// 锁定光标并隐藏。
        /// </summary>
        private void OnEnableMouseControlCamera()
        {
            isRMBPressed = true;
            
            // 将光标锁定在屏幕中心并隐藏它
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // 在一帧内禁用鼠标移动以防止跳动
            StartCoroutine(DisableMouseForFrame());
        }

        /// <summary>
        /// 禁用鼠标控制摄像机模式。
        /// 解锁光标并重置摄像机轴。
        /// </summary>
        private void OnDisableMouseControlCamera()
        {
            isRMBPressed = false;
            
            // 解锁光标并使其可见
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            // 重置摄像机轴以防止重新启用鼠标控制时出现跳动
            freeLookVCam.m_XAxis.m_InputAxisValue = 0f;
            freeLookVCam.m_YAxis.m_InputAxisValue = 0f;
        }

        /// <summary>
        /// 协程：在一帧内锁定摄像机移动。
        /// 用于防止光标锁定瞬间的输入突变。
        /// </summary>
        private IEnumerator DisableMouseForFrame()
        {
            cameraMovementLock = true;
            yield return new WaitForEndOfFrame();
            cameraMovementLock = false;
        }
    }
}