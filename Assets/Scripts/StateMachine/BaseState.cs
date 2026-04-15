using UnityEngine;

namespace Platformer
{
    /// <summary>
    /// 基础状态抽象类。
    /// 所有具体状态（如攻击、跳跃）的基类。
    /// 提供通用的动画参数哈希和生命周期方法。
    /// </summary>
    public abstract class BaseState : IState
    {
        // 玩家控制器引用，用于获取输入或改变物理状态
        protected readonly PlayerController player;
        // 动画控制器引用，用于播放动画
        protected readonly Animator animator;

        // 动画状态哈希值，用于优化性能，避免每帧使用字符串查找
        protected static readonly int LocomotionHash = Animator.StringToHash("Locomotion");
        protected static readonly int JumpHash = Animator.StringToHash("Jump");
        protected static readonly int DashHash = Animator.StringToHash("Dash");
        protected static readonly int AttackHash = Animator.StringToHash("Attack");

        // 动画过渡的标准时长
        protected const float crossFadeDuration = 0.1f;

        /// <summary>
        /// 构造函数，注入依赖项。
        /// </summary>
        protected BaseState(PlayerController player, Animator animator)
        {
            this.player = player;
            this.animator = animator;
        }
        
        /// <summary>
        /// 当进入此状态时调用。
        /// 子类重写此方法以初始化状态（如播放动画）。
        /// </summary>
        public virtual void OnEnter()
        {
            // 默认不执行任何操作
        }

        /// <summary>
        /// 每一帧更新逻辑。
        /// </summary>
        public virtual void Update()
        {
            // 默认不执行任何操作
        }

        /// <summary>
        /// 物理更新循环。
        /// 通常用于处理移动或物理相关的逻辑。
        /// </summary>
        public virtual void FixedUpdate()
        {
            // 默认不执行任何操作
        }

        /// <summary>
        /// 当退出此状态时调用。
        /// 子类重写此方法以清理状态。
        /// </summary>
        public virtual void OnExit()
        {
            // 默认不执行任何操作
        }
    }
}