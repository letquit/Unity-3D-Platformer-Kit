using System;
using Cinemachine;
using KBCore.Refs;
using UnityEngine;

namespace Platformer
{
    public class PlayerController : ValidatedMonoBehaviour
    {
        [Header("References")] 
        [SerializeField, Self] private CharacterController controller;
        [SerializeField, Self] private Animator animator;
        [SerializeField, Anywhere] private CinemachineFreeLook freeLookVCam;
        [SerializeField, Anywhere] private InputReader input;

        [Header("Settings")] 
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float rotationSpeed = 15f;
        [SerializeField] private float smoothTime = 0.2f;

        private const float ZeroF = 0f;
        
        private Transform mainCam;

        private float currentSpeed;
        private float velocity;
        
        // Animator 参数
        private static readonly int Speed = Animator.StringToHash("Speed");
        
        private void Awake()
        {
            mainCam = Camera.main.transform;
            freeLookVCam.Follow = transform;
            freeLookVCam.LookAt = transform;
            // 当观察对象发生瞬移时触发事件，相应地调整 freeLookVCam 的位置
            freeLookVCam.OnTargetObjectWarped(transform,
                transform.position - freeLookVCam.transform.position - Vector3.forward);
        }

        private void Start() => input.EnablePlayerActions();

        private void Update()
        {
            HandleMovement();
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            animator.SetFloat(Speed, currentSpeed);
        }

        private void HandleMovement()
        {
            var movementDirection = new Vector3(input.Direction.x, 0f, input.Direction.y).normalized;
            // 旋转移动方向以匹配摄像机旋转
            var adjustedDirection = Quaternion.AngleAxis(mainCam.eulerAngles.y, Vector3.up) * movementDirection;
            if (adjustedDirection.magnitude > ZeroF)
            {
                HandleRotation(adjustedDirection);
                HandleCharacterController(adjustedDirection);
                SmoothSpeed(adjustedDirection.magnitude);
            }
            else
            {
                SmoothSpeed(ZeroF);
            }
        }

        private void HandleCharacterController(Vector3 adjustedDirection)
        {
            // 移动玩家
            var adjustedMovement = adjustedDirection * (moveSpeed * Time.deltaTime);
            controller.Move(adjustedMovement);
        }

        private void HandleRotation(Vector3 adjustedDirection)
        {
            // 调整旋转以匹配移动方向
            var targetRotation = Quaternion.LookRotation(adjustedDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.LookAt(transform.position + adjustedDirection);
        }

        private void SmoothSpeed(float value)
        {
            currentSpeed = Mathf.SmoothDamp(currentSpeed, value, ref velocity, smoothTime);
        }
    }
}