using System;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.AI;
using Utilities;

namespace Platformer
{
    /// <summary>
    /// 敌人控制器。
    /// 管理敌人的AI行为，包括巡逻、追逐和攻击。
    /// 使用状态机模式组织行为逻辑。
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(PlayerDetector))]
    public class Enemy : Entity
    {
        [SerializeField, Self] private NavMeshAgent agent; // 寻路代理组件
        [SerializeField, Self] private PlayerDetector playerDetector; // 玩家检测器
        [SerializeField, Child] private Animator animator; // 动画组件

        [SerializeField] private float wanderRadius = 10f; // 巡逻半径
        [SerializeField] private float timeBetweenAttacks = 1f; // 攻击间隔
        
        private StateMachine stateMachine; // 状态机实例

        private CountdownTimer attackTimer; // 攻击冷却计时器
        
        private void OnValidate() => this.ValidateRefs(); // 编辑器验证引用

        private void Start()
        {
            attackTimer = new CountdownTimer(timeBetweenAttacks);
            
            stateMachine = new StateMachine();
            
            // 实例化具体状态
            var wanderState = new EnemyWanderState(this, animator, agent, wanderRadius);
            var chaseState = new EnemyChaseState(this, animator, agent, playerDetector.Player);
            var attackState = new EnemyAttackState(this, animator, agent, playerDetector.Player);
            
            // 配置状态转换规则
            At(wanderState, chaseState, new FuncPredicate(() => playerDetector.CanDetectPlayer()));
            At(chaseState, wanderState, new FuncPredicate(() => !playerDetector.CanDetectPlayer()));
            At(chaseState, attackState, new FuncPredicate(() => playerDetector.CanAttackPlayer()));
            At(attackState, chaseState, new FuncPredicate(() => !playerDetector.CanAttackPlayer()));
            
            // 设置初始状态
            stateMachine.SetState(wanderState);
        }

        /// <summary>
        /// 辅助方法：添加状态转换。
        /// </summary>
        private void At(IState from, IState to, IPredicate condition) =>
            stateMachine.AddTransition(from, to, condition);

        /// <summary>
        /// 辅助方法：添加任意转换。
        /// </summary>
        private void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);

        private void Update()
        {
            stateMachine.Update();
            attackTimer.Tick(Time.deltaTime);
        }
        
        private void FixedUpdate()
        {
            stateMachine.FixedUpdate();
        }

        /// <summary>
        /// 执行攻击逻辑。
        /// </summary>
        public void Attack()
        {
            if (attackTimer.IsRunning) return; // 冷却中则不攻击
            
            attackTimer.Start();
            playerDetector.PlayerHealth.TakeDamage(10);
        }
    }
}