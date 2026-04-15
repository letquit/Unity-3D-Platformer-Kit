using UnityEngine;

namespace Platformer
{
    /// <summary>
    /// 敌人基础状态抽象类。
    /// 所有敌人具体状态的基类，提供共有的引用、动画哈希和生命周期方法。
    /// </summary>
    public abstract class EnemyBaseState : IState
    {
        protected readonly Enemy enemy; // 敌人控制器引用
        protected readonly Animator animator; // 动画控制器引用
        
        // 动画状态哈希值缓存，用于性能优化
        protected static readonly int IdleHash = Animator.StringToHash("IdleNormal");
        protected static readonly int RunHash = Animator.StringToHash("RunFWD");
        protected static readonly int WalkHash = Animator.StringToHash("WalkFWD");
        protected static readonly int AttackHash = Animator.StringToHash("Attack01");
        protected static readonly int DieHash = Animator.StringToHash("Die");
        
        // 动画混合过渡时间
        protected const float crossFadeDuration = 0.1f;

        protected EnemyBaseState(Enemy enemy, Animator animator)
        {
            this.enemy = enemy;
            this.animator = animator;
        }
        
        public virtual void OnEnter()
        {
            // 进入状态时的逻辑，默认为空
        }

        public virtual void Update()
        {
            // 每帧更新逻辑，默认为空
        }

        public virtual void FixedUpdate()
        {
            // 物理更新逻辑，默认为空
        }

        public virtual void OnExit()
        {
            // 退出状态时的逻辑，默认为空
        }
    }
}