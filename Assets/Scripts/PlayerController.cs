using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Cinemachine;
using KBCore.Refs;
using UnityEngine;
using Utilities;

namespace Platformer
{
    public class PlayerController : ValidatedMonoBehaviour
    {
        [Header("References")] 
        [SerializeField, Self] private Rigidbody rb;
        [SerializeField, Self] private GroundChecker groundChecker;
        [SerializeField, Self] private Animator animator;
        [SerializeField, Anywhere] private CinemachineFreeLook freeLookVCam;
        [SerializeField, Anywhere] private InputReader input;

        [Header("Movement Settings")] 
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float rotationSpeed = 15f;
        [SerializeField] private float smoothTime = 0.2f;

        [Header("Jump Settings")] 
        [SerializeField] private float jumpForce = 10f;
        [SerializeField] private float jumpDuration = 0.5f;
        [SerializeField] private float jumpCooldown = 0f;
        [SerializeField] private float jumpMaxHeight = 2f;
        [SerializeField] private float gravityMultiplier = 3f;

        private const float ZeroF = 0f;
        
        private Transform mainCam;

        private float currentSpeed;
        private float velocity;
        private float jumpVelocity;

        private Vector3 movement;

        private List<Timer> timers;
        private CountdownTimer jumpTimer;
        private CountdownTimer jumpCooldownTimer;
        
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

            rb.freezeRotation = true;
            
            // 设置计时器
            jumpTimer = new CountdownTimer(jumpDuration);
            jumpCooldownTimer = new CountdownTimer(jumpCooldown);
            timers = new List<Timer>(2) { jumpTimer, jumpCooldownTimer };

            jumpTimer.OnTimerStart += () => jumpCooldownTimer.Start();
        }

        private void Start() => input.EnablePlayerActions();

        private void OnEnable()
        {
            input.Jump += OnJump;
        }
        
        private void OnDisable()
        {
            input.Jump -= OnJump;
        }

        private void OnJump(bool performed)
        {
            if (performed && !jumpTimer.IsRunning && !jumpCooldownTimer.IsRunning && groundChecker.IsGrounded)
            {
                jumpTimer.Start();
            }
            else if (!performed && jumpTimer.IsRunning)
            {
                jumpTimer.Stop();
            }
        }

        private void Update()
        {
            movement = new Vector3(input.Direction.x, 0f, input.Direction.y);

            HandleTimers();
            UpdateAnimator();
        }
        
        private void FixedUpdate()
        {
            HandleJump();
            HandleMovement();
        }

        private void UpdateAnimator()
        {
            animator.SetFloat(Speed, currentSpeed);
        }

        private void HandleTimers()
        {
            foreach (var timer in timers)
            {
                timer.Tick(Time.deltaTime);
            }
        }

        private void HandleJump()
        {
            // If not jumping and grounded, keep jump velocity at 0
            if (!jumpTimer.IsRunning && groundChecker.IsGrounded)
            {
                jumpVelocity = ZeroF;
                jumpTimer.Stop();
                return;
            }
            
            // If jumping or falling calculate velocity
            if (jumpTimer.IsRunning)
            {
                // Progress point for initial burst of velocity
                float launchPoint = 0.9f;
                if (jumpTimer.Progress > launchPoint)
                {
                    // Calculate the velocity required to reach the jump height using equations v = sqrt(2gh)
                    jumpVelocity = Mathf.Sqrt(2 * jumpMaxHeight * Mathf.Abs(Physics.gravity.y));
                }
                else
                {
                    // Gradually apply less velocity as the jump progresses
                    jumpVelocity += (1 - jumpTimer.Progress) * jumpForce * Time.fixedDeltaTime;
                }
            }
            else
            {
                // Gravity takes over
                jumpVelocity += Physics.gravity.y * gravityMultiplier * Time.fixedDeltaTime;
            }
            
            // Apply velocity
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpVelocity, rb.linearVelocity.z);
        }

        private void HandleMovement()
        {
            // 旋转移动方向以匹配摄像机旋转
            var adjustedDirection = Quaternion.AngleAxis(mainCam.eulerAngles.y, Vector3.up) * movement;
            if (adjustedDirection.magnitude > ZeroF)
            {
                HandleRotation(adjustedDirection);
                HandleHorizontalMovement(adjustedDirection);
                SmoothSpeed(adjustedDirection.magnitude);
            }
            else
            {
                SmoothSpeed(ZeroF);
                
                // Reset horizontal velocity for a snappy stop
                rb.linearVelocity = new Vector3(ZeroF, rb.linearVelocity.y, ZeroF);
            }
        }

        private void HandleHorizontalMovement(Vector3 adjustedDirection)
        {
            // 移动玩家
            Vector3 velocity = adjustedDirection * moveSpeed * Time.fixedDeltaTime;
            rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);
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