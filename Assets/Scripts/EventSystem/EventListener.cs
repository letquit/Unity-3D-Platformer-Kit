using System;
using UnityEngine;
using UnityEngine.Events;

namespace Platformer
{
    /// <summary>
    /// 泛型事件监听器基类。
    /// 挂载到 GameObject 上，用于监听特定的 EventChannel 并触发 UnityEvent。
    /// </summary>
    /// <typeparam name="T">监听的数据类型</typeparam>
    public abstract class EventListener<T> : MonoBehaviour
    {
        [SerializeField] private EventChannel<T> eventChannel; // 关联的事件通道（在编辑器中赋值）
        [SerializeField] private UnityEvent<T> unityEvent;     // 当事件触发时调用的 Unity 事件（可在编辑器中配置回调）

        /// <summary>
        /// 初始化时自动注册到事件通道。
        /// </summary>
        protected void Awake()
        {
            eventChannel.Register(this);
        }
        
        /// <summary>
        /// 销毁时自动从事件通道取消注册，防止内存泄漏。
        /// </summary>
        protected void OnDestroy()
        {
            eventChannel.Deregister(this);
        }

        /// <summary>
        /// 由 EventChannel 调用。
        /// 触发本地的 UnityEvent，执行编辑器中配置的逻辑。
        /// </summary>
        /// <param name="value">事件携带的数据</param>
        public void Raise(T value)
        {
            unityEvent?.Invoke(value);
        }
    }
    
    /// <summary>
    /// 无参事件监听器。
    /// 用于监听不需要传递数据的事件（如“游戏开始”、“关卡结束”）。
    /// 继承自 EventListener<Empty>。
    /// </summary>
    public class EventListener : EventListener<Empty> { }
}