using UnityEngine;

namespace Platformer
{
    /// <summary>
    /// 状态接口。
    /// 定义了有限状态机中每个状态必须遵循的生命周期契约。
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// 当进入此状态时调用。
        /// 用于初始化状态相关的逻辑。
        /// </summary>
        void OnEnter();

        /// <summary>
        /// 每一帧调用。
        /// 用于处理非物理的更新逻辑。
        /// </summary>
        void Update();

        /// <summary>
        /// 固定时间间隔调用（物理循环）。
        /// 用于处理物理相关的逻辑，如移动或碰撞检测。
        /// </summary>
        void FixedUpdate();

        /// <summary>
        /// 当退出此状态时调用。
        /// 用于清理状态相关的逻辑。
        /// </summary>
        void OnExit();
    }
}