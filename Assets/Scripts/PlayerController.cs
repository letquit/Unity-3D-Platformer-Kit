using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Cinemachine;
using KBCore.Refs;
using UnityEngine;
using Utilities;

namespace Platformer
{
    /// <summary>
    /// 玩家控制器。
    /// 整合状态机、输入、物理和动画，处理玩家的所有行为。
    /// </summary>
    public class PlayerController : ValidatedMonoBehaviour
    {
        [Header("References")] 
        [SerializeField, Self] private Rigidbody rb; // 刚体组件
        [SerializeField, Self] private GroundChecker groundChecker; // 地面检测器
        [SerializeField, Self] private Animator animator; // 动画控制器
        [SerializeField, Anywhere] private CinemachineFreeLook freeLookVCam; // 摄像机
        [SerializeField, Anywhere] private InputReader input; // 输入读取器

        [Header("Movement Settings")] 
        [SerializeField] private float moveSpeed = 6f; // 移动速度
        [SerializeField] private float rotationSpeed = 15f; // 旋转速度
        [SerializeField] private float smoothTime = 0.2f; // 速度平滑时间

        [Header("Jump Settings")] 
        [SerializeField] private float jumpForce = 10f; // 跳跃力度
        [SerializeField] private float jumpDuration = 0.5f; // 跳跃持续时间（用于蓄力）
        [SerializeField] private float jumpCooldown = 0f; // 跳跃冷却
        [SerializeField] private float gravityMultiplier = 3f; // 重力倍率

        [Header("Dash Settings")]
        [SerializeField] private float dashForce = 10f; // 冲刺力度
        [SerializeField] private float dashDuration = 1f; // 冲刺持续时间
        [SerializeField] private float dashCooldown = 2f; // 冲刺冷却

        [Header("Attack Settings")] 
        [SerializeField] private float attackCooldown = 0.5f; // 攻击冷却
        [SerializeField] private float attackDistance = 1f; // 攻击范围
        [SerializeField] private int attackDamage = 10; // 攻击伤害
        
        private const float ZeroF = 0f;
        
        private Transform mainCam;

        private float currentSpeed;
        private float velocity;
        private float jumpVelocity;
        private float dashVelocity = 1f;

        private Vector3 movement;

        private List<Timer> timers;
        private CountdownTimer jumpTimer;
        private CountdownTimer jumpCooldownTimer;
        private CountdownTimer dashTimer;
        private CountdownTimer dashCooldownTimer;
        private CountdownTimer attackTimer;
        
        private StateMachine stateMachine;
        
        // Animator 参数哈希
        private static readonly int Speed = Animator.StringToHash("Speed");
        
        private void Awake()
        {
            mainCam = Camera.main.transform;
            // 设置摄像机跟随目标
            freeLookVCam.Follow = transform;
            freeLookVCam.LookAt = transform;
            // 处理瞬移时的摄像机修正
            freeLookVCam.OnTargetObjectWarped(transform,
                transform.position - freeLookVCam.transform.position - Vector3.forward);

            rb.freezeRotation = true; // 冻结刚体旋转，防止翻倒
            
            SetupTimers();
            SetupStateMachine();
        }

        /// <summary>
        /// 设置状态机及其转换逻辑。
        /// </summary>
        private void SetupStateMachine()
        {
            stateMachine = new StateMachine();
            
            // 声明状态
            var locomotionState = new LocomotionState(this, animator);
            var jumpState = new JumpState(this, animator);
            var dashState = new DashState(this, animator);
            var attackState = new AttackState(this, animator);
            
            // 定义状态转换
            At(locomotionState, jumpState, new FuncPredicate(() => jumpTimer.IsRunning));
            At(locomotionState, dashState, new FuncPredicate(() => dashTimer.IsRunning));
            At(locomotionState, attackState, new FuncPredicate(() => attackTimer.IsRunning));
            At(attackState, locomotionState, new FuncPredicate(() => !attackTimer.IsRunning));
            // 任意状态在满足条件时返回移动状态
            Any(locomotionState, new FuncPredicate(ReturnToLocomotionState));
            
            stateMachine.SetState(locomotionState);
        }

        private bool ReturnToLocomotionState()
        {
            return groundChecker.IsGrounded && !attackTimer.IsRunning && !jumpTimer.IsRunning && !dashTimer.IsRunning;
        }

        /// <summary>
        /// 初始化所有计时器。
        /// </summary>
        private void SetupTimers()
        {
            jumpTimer = new CountdownTimer(jumpDuration);
            jumpCooldownTimer = new CountdownTimer(jumpCooldown);
            
            jumpTimer.OnTimerStart += () => jumpVelocity = jumpForce;
            jumpTimer.OnTimerStop += () => jumpCooldownTimer.Start();

            dashTimer = new CountdownTimer(dashDuration);
            dashCooldownTimer = new CountdownTimer(dashCooldown);
            dashTimer.OnTimerStart += () => dashVelocity = dashForce;
            dashTimer.OnTimerStop += ()
 =>
            {
                dashVelocity = 1f;
                dashCooldownTimer.Start();
            };

            attackTimer = new CountdownTimer(attackCooldown);
            
            timers = new List<Timer>(5) { jumpTimer, jumpCooldownTimer, dashTimer, dashCooldownTimer, attackTimer };
        }

        private void At(IState from, IState to, IPredicate condition) =>
            stateMachine.AddTransition(from, to, condition);

        private void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);

        private void Start() => input.EnablePlayerActions();

        private void OnEnable()
        {
            input.Jump += OnJump;
            input.Dash += OnDash;
            input.Attack += OnAttack;
        }
        
        private void OnDisable()
        {
            input.Jump -= OnJump;
            input.Dash -= OnDash;
            input.Attack -= OnAttack;
        }

        /// <summary>
        /// 处理攻击输入事件。
        /// </summary>
        private void OnAttack()
        {
            if (!attackTimer.IsRunning)
            {
                attackTimer.Start();
            }
        }

        /// <summary>
        /// 执行攻击逻辑。
        /// </summary>
        public void Attack()
        {
            Vector3 attackPos = transform.position + transform.forward;
            Collider[] hitEnemies = Physics.OverlapSphere(attackPos, attackDistance);

            foreach (var enemy in hitEnemies)
            {
                Debug.Log(enemy.name);
                if (enemy.CompareTag("Enemy"))
                {
                    enemy.GetComponent<Health>().TakeDamage(attackDamage);
                }
            }
        }

        /// <summary>
        /// 处理跳跃输入事件。
        /// </summary>
        /// <param name="performed">跳跃操作是否被执行</param>
        private void OnJump(bool performed)
        {
            // 按下跳跃键且满足条件时开始跳跃
            if (performed && !jumpTimer.IsRunning && !jumpCooldownTimer.IsRunning && groundChecker.IsGrounded)
            {
                jumpTimer.Start();
            }
            // 松开跳跃键时提前结束跳跃（实现可变高度）
            else if (!performed && jumpTimer.IsRunning)
            {
                jumpTimer.Stop();
            }
        }

        /// <summary>
        /// 处理冲刺输入事件。
        /// </summary>
        /// <param name="performed">冲刺操作是否被执行</param>
        private void OnDash(bool performed)
        {
            if (performed && !dashTimer.IsRunning && !dashCooldownTimer.IsRunning)
            {
                dashTimer.Start();
            }
            else if (!performed && dashTimer.IsRunning)
            {
                dashTimer.Stop();
            }
        }

        private void Update()
        {
            movement = new Vector3(input.Direction.x, 0f, input.Direction.y);
            stateMachine.Update();

            HandleTimers();
            UpdateAnimator();
        }
        
        private void FixedUpdate()
        {
            stateMachine.FixedUpdate();
        }

        private void UpdateAnimator()
        {
            animator.SetFloat(Speed, currentSpeed);
        }

        /// <summary>
        /// 更新所有计时器。
        /// </summary>
        private void HandleTimers()
        {
            foreach (var timer in timers)
            {
                timer.Tick(Time.deltaTime);
            }
        }

        /// <summary>
        /// 处理跳跃物理逻辑。
        /// </summary>
        public void HandleJump()
        {
            if (!jumpTimer.IsRunning && groundChecker.IsGrounded)
            {
                jumpVelocity = ZeroF;
                return;
            }
            
            if (!jumpTimer.IsRunning)
            {
                // 应用重力
                jumpVelocity += Physics.gravity.y * gravityMultiplier * Time.fixedDeltaTime;
            }
            
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpVelocity, rb.linearVelocity.z);
        }

        /// <summary>
        /// 处理水平移动逻辑。
        /// </summary>
        public void HandleMovement()
        {
            // 根据摄像机角度调整移动方向
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
                // 停止时重置水平速度
                rb.linearVelocity = new Vector3(ZeroF, rb.linearVelocity.y, ZeroF);
            }
        }

        /// <summary>
        /// 处理水平方向移动。
        /// </summary>
        /// <param name="adjustedDirection">调整后的移动方向</param>
        private void HandleHorizontalMovement(Vector3 adjustedDirection)
        {
            // 计算并应用水平速度
            Vector3 velocity = adjustedDirection * (moveSpeed * dashVelocity * Time.fixedDeltaTime);
            rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);
        }

        /// <summary>
        /// 处理角色旋转。
        /// </summary>
        /// <param name="adjustedDirection">调整后的移动方向</param>
        private void HandleRotation(Vector3 adjustedDirection)
        {
            // 平滑旋转角色面向移动方向
            var targetRotation = Quaternion.LookRotation(adjustedDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.LookAt(transform.position + adjustedDirection);
        }

        /// <summary>
        /// 平滑过渡当前速度。
        /// </summary>
        /// <param name="value">目标速度值</param>
        private void SmoothSpeed(float value)
        {
            // 平滑过渡速度值用于动画
            currentSpeed = Mathf.SmoothDamp(currentSpeed, value, ref velocity, smoothTime);
        }
    }
}