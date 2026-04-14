using System;
using System.Collections;
using Cinemachine;
using KBCore.Refs;
using UnityEngine;

namespace Platformer
{
    public class CameraManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField, Anywhere] private InputReader input;
        [SerializeField, Anywhere] private CinemachineFreeLook freeLookVCam;

        [Header("Settings")]
        [SerializeField, Range(0.5f, 3f)] private float speedMultiplier = 1f;

        private bool isRMBPressed;
        private bool cameraMovementLock;

        private void OnEnable()
        {
            input.Look += OnLook;
            input.EnableMouseControlCamera += OnEnableMouseControlCamera;
            input.DisableMouseControlCamera += OnDisableMouseControlCamera;
        }

        private void OnDisable()
        {
            input.Look -= OnLook;
            input.EnableMouseControlCamera -= OnEnableMouseControlCamera;
            input.DisableMouseControlCamera -= OnDisableMouseControlCamera;
        }

        private void OnLook(Vector2 cameraMovement, bool isDeviceMouse)
        {
            if (cameraMovementLock) return;
            
            if (isDeviceMouse && !isRMBPressed) return;
            
            // 如果设备是鼠标则使用 fixedDeltaTime，否则使用 deltaTime
            float deviceMultiplier = isDeviceMouse ? Time.fixedDeltaTime : Time.deltaTime;
            
            // 设置摄像机轴的值
            freeLookVCam.m_XAxis.m_InputAxisValue = cameraMovement.x * speedMultiplier * deviceMultiplier;
            freeLookVCam.m_YAxis.m_InputAxisValue = cameraMovement.y * speedMultiplier * deviceMultiplier;
        }

        private void OnEnableMouseControlCamera()
        {
            isRMBPressed = true;
            
            // 将光标锁定在屏幕中心并隐藏它
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            StartCoroutine(DisableMouseForFrame());
        }

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

        private IEnumerator DisableMouseForFrame()
        {
            cameraMovementLock = true;
            yield return new WaitForEndOfFrame();
            cameraMovementLock = false;
        }
    }
}