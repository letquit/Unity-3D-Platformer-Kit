using System;

namespace Utilities
{
    /// <summary>
    /// 计时器抽象基类。
    /// 定义了计时器的通用状态、事件和生命周期管理。
    /// </summary>
    public abstract class Timer
    {
        protected float initialTime; // 初始设定的时间
        protected float Time { get; set; } // 当前剩余或已流逝的时间
        public bool IsRunning { get; protected set; } // 计时器是否正在运行
        
        /// <summary>
        /// 获取当前进度（0到1之间）。
        /// </summary>
        public float Progress => Time / initialTime;

        // 事件回调，使用空委托避免空引用检查
        public Action OnTimerStart = delegate { };
        public Action OnTimerStop = delegate { };

        protected Timer(float value)
        {
            initialTime = value;
            IsRunning = false;
        }
        
        /// <summary>
        /// 启动计时器。
        /// 重置时间并触发开始事件。
        /// </summary>
        public void Start()
        {
            Time = initialTime;
            if (!IsRunning)
            {
                IsRunning = true;
                OnTimerStart.Invoke();
            }
        }

        /// <summary>
        /// 停止计时器。
        /// 停止运行并触发停止事件。
        /// </summary>
        public void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;
                OnTimerStop.Invoke();
            }
        }

        public void Resume() => IsRunning = true; // 恢复运行
        public void Pause() => IsRunning = false; // 暂停运行

        /// <summary>
        /// 每帧更新逻辑。
        /// 由子类实现具体的时间增减逻辑。
        /// </summary>
        /// <param name="deltaTime">上一帧到当前帧的时间间隔</param>
        public abstract void Tick(float deltaTime);
    }

    /// <summary>
    /// 倒计时器。
    /// 时间从初始值递减至零。
    /// </summary>
    public class CountdownTimer : Timer
    {
        public CountdownTimer(float value) : base(value) { }

        public override void Tick(float deltaTime)
        {
            if (IsRunning && Time > 0)
            {
                Time -= deltaTime;
            }

            // 时间耗尽自动停止
            if (IsRunning && Time <= 0)
            {
                Stop();
            }
        }
        
        public bool IsFinished() => Time <= 0; // 检查是否完成
        
        public void Reset() => Time = initialTime; // 重置时间

        /// <summary>
        /// 重置并设定新的初始时间。
        /// </summary>
        public void Reset(float newTime)
        {
            initialTime = newTime;
            Reset();
        }
    }

    /// <summary>
    /// 秒表计时器。
    /// 时间从零开始递增。
    /// </summary>
    public class StopwatchTimer : Timer
    {
        public StopwatchTimer() : base(0) { }

        public override void Tick(float deltaTime)
        {
            if (IsRunning)
            {
                Time += deltaTime;
            }
        }
        
        public void Reset() => Time = 0; // 重置时间
        
        public float GetTime() => Time; // 获取当前时间
    }
}